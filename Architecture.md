# Project Architecture & Developer Guidelines

## 1. Tech Stack

- **Backend:** .NET Web API
- **Database:** MSSQL (Entity Framework Core — Fluent API)
- **Caching:** Redis
- **Message Broker:** RabbitMQ
- **Frontend (Future):** React / Next.js
- **Architecture Pattern:** Clean Architecture

---

## 2. Clean Architecture Layers & Rules

This project strictly follows Clean Architecture principles. Do not violate layer dependencies.

- **Domain Layer:** Contains only Business Rules, Entities, Enums, and Value Objects.
  **NO** external dependencies (no Entity Framework, no third-party libraries).

- **Application Layer:** Contains Use Cases, DTOs, CQRS (Commands/Queries via MediatR), and Interfaces (e.g., `IRepository<T>`).
  References **only** the Domain layer.
  All command/query handlers return `Result` or `Result<TData>`.

- **Infrastructure Layer:** Contains implementations of Interfaces (e.g., `UserRepository`, `RedisCacheService`, `RabbitMQPublisher`).
  Uses EF Core with **Fluent API only** — no Data Annotations on entities.
  References Application layer.

- **API (Presentation) Layer:** Contains Controllers / Minimal APIs, Middlewares, Exception Filters, and DI configurations.
  References Application and Infrastructure layers.

---

## 3. Coding Principles

### 3.1 General
- **SOLID & DRY:** Strictly adhere to SOLID principles. Do not repeat code.
- **Dependency Injection:** Always use DI. Never use the `new` keyword for services.
- **Method size:** Keep methods small and focused on a single responsibility.

### 3.2 Naming Conventions
| Element | Convention | Example |
|---|---|---|
| Interfaces | Prefix with `I` | `IProductRepository` |
| Async methods | Suffix with `Async` | `GetProductByIdAsync` |
| Classes & Methods | PascalCase | `ProductService`, `GetAll` |
| Local variables | camelCase | `productList` |
| EF entity configs | `{Entity}Configuration` | `ProductConfiguration` |
| Commands | `{Action}{Entity}Command` | `CreateProductCommand` |
| Queries | `{Action}{Entity}Query` | `GetProductByIdQuery` |
| Handlers | `{CommandOrQuery}Handler` | `CreateProductCommandHandler` |

### 3.3 Encapsulation
- Entities must have **private setters**.
- Mutate state only via constructors or specific domain methods.
- Example:
  ```csharp
  // CORRECT
  public class Product
  {
      public Guid Id { get; private set; }
      public string Name { get; private set; }

      public Product(Guid id, string name)
      {
          Id = id;
          Name = name;
      }

      public void UpdateName(string name) => Name = name;
  }
  ```

---

## 4. Result Pattern

All methods that can fail **must** return `Result` or `Result<TData>`. Do **not** throw exceptions for business logic failures.

### 4.1 Class Structure (Do not modify)
Located at: `Inventra.Application.Common.Results`

```csharp
// Non-generic: for operations that return no data (e.g., delete, update)
Result.Success()
Result.Success("Custom message.")
Result.Failure("Something went wrong.")
Result.Failure(new[] { "Error 1", "Error 2" })

// Generic: for operations that return data (e.g., get, create)
Result.Success<TData>(data)
Result.Success<TData>(data, "Custom message.")
Result.Failure<TData>("Something went wrong.")
Result.Failure<TData>(new[] { "Error 1", "Error 2" })
```

### 4.2 Usage Rules
- **Command Handlers** → return `Result`
- **Query Handlers** → return `Result<TData>`
- Never return `null`. Use `Result.Failure<TData>("Not found.")` instead.
- Do not wrap `Result` in another `Result`.

### 4.3 Handler Example
```csharp
// Command Handler (no data returned)
public async Task<Result> Handle(CreateProductCommand request, CancellationToken cancellationToken)
{
    var exists = await _repository.ExistsAsync(request.Name, cancellationToken);
    if (exists)
        return Result.Failure("A product with this name already exists.");

    var product = new Product(Guid.NewGuid(), request.Name);
    await _repository.AddAsync(product, cancellationToken);
    return Result.Success("Product created successfully.");
}

// Query Handler (data returned)
public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
{
    var product = await _repository.GetByIdAsync(request.Id, cancellationToken);
    if (product is null)
        return Result.Failure<ProductDto>("Product not found.");

    return Result.Success(new ProductDto(product.Id, product.Name));
}
```

### 4.4 Controller Mapping
Map `Result` to HTTP responses in the controller, never in the handler.

```csharp
[HttpPost]
public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
{
    var result = await _mediator.Send(command);
    return result.IsSuccess
        ? Ok(result.Message)
        : BadRequest(result.Errors);
}

[HttpGet("{id}")]
public async Task<IActionResult> GetById(Guid id)
{
    var result = await _mediator.Send(new GetProductByIdQuery(id));
    return result.IsSuccess
        ? Ok(result.Data)
        : NotFound(result.Errors);
}
```

---

## 5. CQRS & Mediator

- Use **MediatR** for all commands and queries.
- Commands implement `IRequest<Result>`, Queries implement `IRequest<Result<TData>>`.
- Commands must **not** return business data — only `Result` (success/failure).
- Queries must have **no side effects**.
- Use **FluentValidation** with MediatR pipeline behaviors for input validation. Keep validation logic out of handlers.

```csharp
// Command
public record CreateProductCommand(string Name, decimal Price) : IRequest<Result>;

// Query
public record GetProductByIdQuery(Guid Id) : IRequest<Result<ProductDto>>;
```

---

## 6. EF Core Conventions

- **Fluent API only.** Do not use Data Annotations (`[Required]`, `[MaxLength]`, etc.) on entity classes.
- All entity configurations must be in separate classes implementing `IEntityTypeConfiguration<T>`.
- Register configurations via `modelBuilder.ApplyConfigurationsFromAssembly(...)` in `DbContext`.
- **Soft delete** is preferred over hard delete where applicable. Add an `IsDeleted` filter via global query filters.
- Always define explicit table names and column names in configuration.

```csharp
// CORRECT — Separate configuration class
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("Name");

        builder.Property(p => p.Price)
            .HasColumnType("decimal(18,2)");

        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}
```

---

## 7. Exception Handling

### 7.1 Responsibility Separation (Critical)

This project uses **two distinct mechanisms** for error handling. They must never be mixed.

| Error Type | Examples | How to Handle |
|---|---|---|
| Business logic failure | Product not found, duplicate name, validation error | `Result.Failure(...)` — no exception |
| Infrastructure failure | DB connection lost, Redis timeout, null reference bug | Exception → caught by `GlobalExceptionFilter` |

**The rule is simple:** if you can anticipate the failure and it's part of normal business flow, use `Result`. If the application genuinely crashed due to something unexpected, the filter catches it.

### 7.2 What NOT to do

```csharp
// ❌ WRONG — throwing for a business rule
public async Task<Result> Handle(CreateProductCommand request, CancellationToken ct)
{
    var exists = await _repository.ExistsAsync(request.Name, ct);
    if (exists)
        throw new DomainException("Product already exists."); // Never do this
}

// ❌ WRONG — catching business errors in the filter
// Do not add cases like DomainException, ValidationException,
// KeyNotFoundException to GlobalExceptionFilter. These should never throw.

// ✅ CORRECT
public async Task<Result> Handle(CreateProductCommand request, CancellationToken ct)
{
    var exists = await _repository.ExistsAsync(request.Name, ct);
    if (exists)
        return Result.Failure("A product with this name already exists.");

    var product = new Product(Guid.NewGuid(), request.Name);
    await _repository.AddAsync(product, ct);
    return Result.Success("Product created successfully.");
}
```

### 7.3 Global Exception Filter

The filter is a **last resort** for unhandled infrastructure exceptions only. It must stay simple — no business-specific exception types.

```csharp
public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;
    private readonly IWebHostEnvironment _environment;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger, IWebHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(
            context.Exception,
            "Unhandled infrastructure exception. TraceId: {TraceId}",
            context.HttpContext.TraceIdentifier);

        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An unexpected error occurred.",
            Instance = context.HttpContext.Request.Path,
            Detail = _environment.IsDevelopment() ? context.Exception.Message : null
        };

        context.Result = new ObjectResult(problem) { StatusCode = 500 };
        context.ExceptionHandled = true;
    }
}
```

Register in `Program.cs`:

```csharp
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});
```

---

## 8. GitHub Copilot Instructions

When generating code for this repository, strictly follow all rules above. Additional constraints:

1. **Layer check first:** Before writing any code, determine which layer the file belongs to. Verify that any reference or dependency is allowed for that layer.

2. **Result Pattern always:** Every method that can fail must return `Result` or `Result<TData>`. Never throw exceptions for business failures. Never return `null`.

3. **No Data Annotations on entities:** Use Fluent API in a separate `IEntityTypeConfiguration<T>` class.

4. **No `new` for services:** Always inject dependencies via constructor.

5. **EF Core in Infrastructure only:** `DbContext`, migrations, and EF-related code must never appear in Domain or Application layers.

6. **Async all the way:** All I/O operations must be `async/await`. Method names must end with `Async`.

7. **MediatR for all requests:** Do not call service classes directly from controllers. Always dispatch via `IMediator.Send(...)`.

8. **FluentValidation for inputs:** Do not validate inside handlers. Use a `ValidationBehavior` pipeline.

9. **No business logic in controllers:** Controllers only map `Result` to HTTP responses.

10. **Exception filter is last resort only:** `GlobalExceptionFilter` catches only unexpected infrastructure failures (DB crash, null reference, etc.). Do **not** add `DomainException`, `ValidationException`, `KeyNotFoundException`, or any business-related exception type to the filter. These must never be thrown — use `Result.Failure(...)` instead.

11. **Never throw for business failures:** Do not create or throw custom domain/business exception classes. If you find yourself writing `throw new SomethingException(...)` inside a handler or service, stop — return `Result.Failure(...)` instead.
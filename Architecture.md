# Project Architecture & Developer Guidelines

## 1. Tech Stack
*   **Backend:** .NET Web API
*   **Database:** MSSQL (Entity Framework Core)
*   **Caching:** Redis
*   **Message Broker:** RabbitMQ
*   **Frontend (Future):** React / Next.js
*   **Architecture Pattern:** Clean Architecture

## 2. Clean Architecture Layers & Rules
This project strictly follows Clean Architecture principles. Do not violate layer dependencies.

*   **Domain Layer:** Contains only Business Rules, Entities, Enums, and Value Objects. **NO** external dependencies (No Entity Framework, no third-party libraries).
*   **Application Layer:** Contains Use Cases, DTOs, CQRS (Commands/Queries), and Interfaces (e.g., `IRepository`). References ONLY the Domain layer.
*   **Infrastructure Layer:** Contains the actual implementations of Interfaces (e.g., `UserRepository`, `RedisCacheService`, `RabbitMQPublisher`). Uses EF Core for MSSQL. References Application layer.
*   **API (Presentation) Layer:** Contains Controllers / Minimal APIs, Middlewares, and Dependency Injection configurations. References Application and Infrastructure layers.

## 3. Coding Principles
*   **SOLID & DRY:** Strictly adhere to SOLID principles. Do not repeat code.
*   **Dependency Injection:** Always use DI. Never use `new` keyword for services.
*   **Naming Conventions:**
    *   Interfaces must start with `I` (e.g., `IProductRepository`).
    *   Async methods must end with `Async` suffix (e.g., `GetProductByIdAsync`).
    *   Use PascalCase for classes and methods, camelCase for local variables.
*   **Encapsulation:** Entities must have private setters. Use constructors or specific domain methods to mutate state.
*   *Return Types: Use Result Pattern for methods that can fail, instead of throwing exceptions directly. This promotes better error handling and clearer intent.
  * Use CQRS pattern for commands and queries. Commands should not return data, only success/failure status. Queries should return data without side effects.
  * Use Mediator pattern for handling commands and queries. This decouples the request from its handling logic.
  * Use FluentValidation for validating commands and queries. Keep validation logic separate from business logic.
  
## 4. GitHub Copilot Instructions
When generating code for this repository:
1. Always verify which layer the file belongs to before adding references.
2. Implement robust error handling using global Middlewares.
3. Keep methods small and focused on a single responsibility.
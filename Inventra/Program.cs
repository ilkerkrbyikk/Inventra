using Inventra.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add exception handling services
builder.Services.AddExceptionHandling();

// Add other services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Use exception handling middleware (early in pipeline)
app.UseExceptionHandling();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

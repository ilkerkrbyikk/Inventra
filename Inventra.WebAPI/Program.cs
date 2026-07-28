using Inventra.Application.Extensions;
using Inventra.Infrastructure;
using Inventra.Infrastructure.Extensions;
using Inventra.WebAPI.Extensions;
using Inventra.WebAPI.Filters;
using Inventra.WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add logging
builder.Services.AddLogging(config =>
{
    config.ClearProviders();
    config.AddConsole();
    config.AddDebug();
});

// Add Application layer services (MediatR, validators, behaviors)
builder.Services.AddApplicationLayer();

// Add Infrastructure layer services (repositories, database, etc.)
builder.Services.AddInfrastructureLayer(builder.Configuration);

// Add API layer services
builder.Services.AddControllers(options =>
{
    // Register global exception filter
    options.Filters.Add<GlobalExceptionFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure request pipeline
app.UseWebAPIConfiguration(app.Environment);

app.MapControllers();

await app.RunAsync();

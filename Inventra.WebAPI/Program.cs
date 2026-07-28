using Inventra.Application.Extensions;
using Inventra.Infrastructure;
using Inventra.Infrastructure.Extensions;
using Inventra.Application.Interfaces;
using Inventra.WebAPI.Extensions;
using Inventra.WebAPI.Filters;
using Inventra.WebAPI.Hubs;
using Inventra.WebAPI.Middleware;
using Inventra.WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add logging
builder.Services.AddLogging(config =>
{
    config.ClearProviders();
    config.AddConsole();
    config.AddDebug();
});

// Add Application layer services (MediatR, validators, behaviors)
builder.Services.AddApplicationLayer(builder.Configuration);

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
builder.Services.AddSignalR();
builder.Services.AddScoped<IInventoryNotificationService, SignalRInventoryNotificationService>();

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
app.MapHub<InventoryNotificationHub>("/hubs/inventory-notifications");

await app.RunAsync();

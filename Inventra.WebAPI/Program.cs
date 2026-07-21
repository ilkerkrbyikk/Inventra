using Inventra.Application;
using Inventra.Infrastructure;
using Inventra.Infrastructure.Persistence;
using Inventra.WebAPI.Extensions;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services from all layers
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddWebAPIServices();

var app = builder.Build();

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DatabaseContext>();
    var logger = services.GetRequiredService<ILogger<DatabaseContext>>();

    await DatabaseInitializer.InitializeAsync(context, logger);
}

// Use WebAPI configuration (includes audit context middleware)
app.UseWebAPIConfiguration(app.Environment);

app.MapControllers();

app.Run();

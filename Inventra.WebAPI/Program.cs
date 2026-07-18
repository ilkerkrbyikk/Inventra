using Inventra.Application;
using Inventra.Infrastructure;
using Inventra.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services from all layers
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddWebAPIServices();

var app = builder.Build();

// Use WebAPI configuration
app.UseWebAPIConfiguration(app.Environment);

app.MapControllers();

app.Run();

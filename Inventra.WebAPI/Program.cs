using Inventra.Application.Features.Procurement.Commands;
using Inventra.Application.Features.StockTransfer.Commands;
using Inventra.Application.Features.StockTransfer.Validators;
using Inventra.Application.Interfaces;
using Inventra.Infrastructure.Persistence;
using Inventra.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database Context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(connectionString));

// Unit of Work & Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProcurementCommand).Assembly));

// FluentValidation
//builder.Services.AddValidatorsFromAssembly(typeof(CreateTransferRequestCommandValidator).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

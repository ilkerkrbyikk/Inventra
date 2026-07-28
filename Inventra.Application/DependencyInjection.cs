using FluentValidation;
using Inventra.Application.Common.Behaviors;
using Inventra.Application.Common.CQRS;
using Inventra.Application.Features.Procurement.Commands;
using Inventra.Application.Features.StockTransfer.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Inventra.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register MediatR with all handlers from this assembly
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(CreateProcurementCommand).Assembly));

            // Register FluentValidation validators from this assembly
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            // Configure validation behavior
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}

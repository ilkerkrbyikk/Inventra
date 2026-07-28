using FluentValidation;
using Inventra.Application.Common.Behaviors;
using Inventra.Application.Common.Options;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventra.Application.Extensions
{
    /// <summary>
    /// Extension methods for registering Application layer services in DI.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Application layer services to the DI container.
        /// Registers MediatR, validators, and pipeline behaviors.
        /// </summary>
        public static IServiceCollection AddApplicationLayer(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Register MediatR
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
                
                // Register pipeline behaviors - order matters
                // Logging happens first, then validation
                config.AddOpenBehavior(typeof(LoggingBehavior<,>));
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            // Register all validators from this assembly
            services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);

            services.Configure<InventoryNotificationOptions>(
                configuration.GetSection(InventoryNotificationOptions.SectionName));

            return services;
        }
    }
}

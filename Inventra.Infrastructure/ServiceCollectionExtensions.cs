using Inventra.Application.Interfaces;
using Inventra.Infrastructure.Audit;
using Inventra.Infrastructure.Persistence;
using Inventra.Infrastructure.Repositories;
using Inventra.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventra.Infrastructure
{
    /// <summary>
    /// Extension methods for registering Infrastructure layer services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds all infrastructure services to the dependency injection container.
        /// Includes database context, repositories, audit services, and interceptors.
        /// </summary>
        /// <param name="services">Service collection to register services.</param>
        /// <param name="configuration">Application configuration.</param>
        /// <returns>Service collection for method chaining.</returns>
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Register database context with audit interceptor
            var sensitivePropertyFilter = new SensitivePropertyFilter();
            var auditChangeTracker = new AuditChangeTracker(sensitivePropertyFilter);
            var auditInterceptor = new AuditSaveChangesInterceptor(auditChangeTracker);

            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection") ?? 
                    throw new InvalidOperationException("Connection string 'DefaultConnection' not found."))
                .AddInterceptors(auditInterceptor));

            // Register Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

            // Register Audit Services
            services.AddSingleton(sensitivePropertyFilter);
            services.AddSingleton(auditChangeTracker);
            services.AddSingleton(auditInterceptor);
            services.AddScoped<IAuditService, AuditService>();

            // Register generic repositories (if not already registered in Application layer)
            // This allows repositories to be injected directly if needed
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return services;
        }
    }
}
using Inventra.Application.Interfaces;
using Inventra.Infrastructure.Persistence;
using Inventra.Infrastructure.Repositories;
using Inventra.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventra.Infrastructure.Extensions
{
    /// <summary>
    /// Extension methods for registering Infrastructure layer services in DI.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Infrastructure layer services to the DI container.
        /// Registers database context, repositories, and related services.
        /// </summary>
        public static IServiceCollection AddInfrastructureLayer(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Register database context
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            // Register generic repository
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Register specific repositories
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

            return services;
        }
    }
}

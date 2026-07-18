using Inventra.Application.Interfaces;
using Inventra.Infrastructure.Extensions;
using Inventra.Infrastructure.Persistence;
using Inventra.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventra.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Register Database Context
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(connectionString));

            // Register Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

            // Register all repositories
            services.AddRepositories();

            return services;
        }
    }
}
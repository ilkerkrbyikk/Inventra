using Inventra.Application.Interfaces;
using Inventra.Domain.Entities;
using Inventra.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Inventra.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds all repository registrations at once.
        /// </summary>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IStockTransactionRepository, StockTransactionRepository>();
            services.AddScoped<IProcurementRepository, ProcurementRepository>();

            return services;
        }
    }
}
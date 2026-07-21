using Inventra.WebAPI.Filters;
using Inventra.WebAPI.Middleware;

namespace Inventra.WebAPI.Extensions
{
    /// <summary>
    /// Extension methods for registering exception handling services.
    /// </summary>
    public static class ExceptionHandlingExtensions
    {
        /// <summary>
        /// Adds global exception handling to the services collection.
        /// Registers the global exception filter for controllers.
        /// </summary>
        /// <param name="services">Service collection to register services.</param>
        /// <returns>Service collection for method chaining.</returns>
        public static IServiceCollection AddExceptionHandling(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            });

            return services;
        }

        /// <summary>
        /// Adds exception handling middleware to the request pipeline.
        /// Should be called early in the pipeline configuration.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <returns>Application builder for method chaining.</returns>
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            return app;
        }
    }
}
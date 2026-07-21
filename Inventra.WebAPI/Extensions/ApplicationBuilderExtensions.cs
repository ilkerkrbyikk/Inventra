using Inventra.WebAPI.Middleware;

namespace Inventra.WebAPI.Extensions
{
    /// <summary>
    /// Extension methods for configuring the request pipeline.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Uses WebAPI middleware and configuration.
        /// Sets up audit context middleware, Swagger, and other pipeline components.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="environment">Hosting environment.</param>
        /// <returns>Application builder for method chaining.</returns>
        public static IApplicationBuilder UseWebAPIConfiguration(
            this IApplicationBuilder app,
            IHostEnvironment environment)
        {
            // Add audit context middleware early in the pipeline
            app.UseMiddleware<AuditContextMiddleware>();

            if (environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Inventra API v1");
                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            // Add CORS
            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
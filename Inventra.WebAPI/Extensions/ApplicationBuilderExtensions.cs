using Inventra.WebAPI.Middleware;

namespace Inventra.WebAPI.Extensions
{
    /// <summary>
    /// Extension methods for configuring the request pipeline.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Configures the HTTP request pipeline with all necessary middleware and routes.
        /// Sets up audit context, Swagger, CORS, authentication, and routing.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="environment">Hosting environment.</param>
        /// <returns>Application builder for method chaining.</returns>
        public static IApplicationBuilder UseWebAPIConfiguration(
            this IApplicationBuilder app,
            IHostEnvironment environment)
        {
            // Audit context middleware captures user and IP info
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

            // CORS must be before auth
            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
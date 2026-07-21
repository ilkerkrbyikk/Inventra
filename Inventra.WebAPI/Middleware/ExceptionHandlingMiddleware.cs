using System.Net;
using System.Text.Json;

namespace Inventra.WebAPI.Middleware
{
    /// <summary>
    /// Middleware for handling unhandled infrastructure exceptions in the request pipeline.
    /// This is a last resort for unexpected errors that bypass normal handling.
    /// Business logic failures must use Result.Failure in handlers, not exceptions.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the ExceptionHandlingMiddleware class.
        /// </summary>
        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IWebHostEnvironment environment)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        /// <summary>
        /// Invokes the middleware to handle unhandled infrastructure exceptions in the request pipeline.
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Unhandled infrastructure exception in middleware pipeline. TraceId: {TraceId}",
                    context.TraceIdentifier);
                
                await HandleExceptionAsync(context, exception);
            }
        }

        /// <summary>
        /// Handles unhandled infrastructure exceptions and writes a generic error response.
        /// </summary>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = new
            {
                status = StatusCodes.Status500InternalServerError,
                title = "An unexpected error occurred.",
                detail = _environment.IsDevelopment() 
                    ? exception.Message 
                    : "An unexpected error occurred while processing your request.",
                traceId = context.TraceIdentifier,
                timestamp = DateTime.UtcNow
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
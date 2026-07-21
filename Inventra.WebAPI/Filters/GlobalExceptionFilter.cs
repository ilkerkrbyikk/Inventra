using Inventra.WebAPI.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Inventra.WebAPI.Filters
{
    /// <summary>
    /// Global exception filter that catches unhandled infrastructure exceptions only.
    /// Business logic failures must use Result.Failure in handlers, not exceptions.
    /// Implements RFC 7807 Problem Details for HTTP APIs.
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;
        private readonly IWebHostEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the GlobalExceptionFilter class.
        /// </summary>
        /// <param name="logger">Logger for recording exception details.</param>
        /// <param name="environment">Hosting environment for determining detail visibility.</param>
        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger, IWebHostEnvironment environment)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        /// <summary>
        /// Called when an unhandled infrastructure exception is thrown during request processing.
        /// This filter is a last resort for unexpected errors only.
        /// Business logic failures must use Result.Failure and never throw.
        /// </summary>
        public void OnException(ExceptionContext context)
        {
            if (context == null)
                return;

            var traceId = context.HttpContext.TraceIdentifier;
            var exceptionType = context.Exception.GetType().Name;

            // Log the unexpected infrastructure exception
            _logger.LogError(
                context.Exception,
                "Unhandled infrastructure exception. Type: {ExceptionType} | TraceId: {TraceId}",
                exceptionType,
                traceId);

            // Create problem details response for infrastructure errors only
            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occurred.",
                Instance = context.HttpContext.Request.Path,
                Detail = _environment.IsDevelopment() ? context.Exception.Message : null
            };

            context.Result = new ObjectResult(problem) { StatusCode = 500 };
            context.ExceptionHandled = true;
        }
    }
}
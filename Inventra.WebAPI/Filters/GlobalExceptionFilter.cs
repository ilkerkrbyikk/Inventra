using Inventra.WebAPI.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Inventra.WebAPI.Filters
{
    /// <summary>
    /// Global exception filter that catches unhandled infrastructure exceptions.
    /// This is a last resort for unexpected errors only.
    /// Business logic failures must use Result.Failure(...) instead.
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;
        private readonly IWebHostEnvironment _environment;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public void OnException(ExceptionContext context)
        {
            var traceId = context.HttpContext.TraceIdentifier;

            _logger.LogError(
                context.Exception,
                "Unhandled infrastructure exception. TraceId: {TraceId}. Request: {Method} {Path}",
                traceId,
                context.HttpContext.Request.Method,
                context.HttpContext.Request.Path);

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occurred.",
                Detail = _environment.IsDevelopment()
                    ? context.Exception.Message
                    : "Please contact support with the trace ID below.",
                Instance = context.HttpContext.Request.Path,
                Extensions = new Dictionary<string, object?>
                {
                    { "traceId", traceId }
                }
            };

            context.Result = new ObjectResult(problemDetails) { StatusCode = 500 };
            context.ExceptionHandled = true;
        }
    }
}
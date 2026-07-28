using MediatR;
using Microsoft.Extensions.Logging;

namespace Inventra.Application.Common.Behaviors
{
    /// <summary>
    /// MediatR pipeline behavior for logging requests and responses.
    /// Logs request start, execution time, and response status.
    /// </summary>
    /// <typeparam name="TRequest">Type of request being logged.</typeparam>
    /// <typeparam name="TResponse">Type of response from the handler.</typeparam>
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : notnull
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var requestId = Guid.NewGuid();

            _logger.LogInformation(
                "Handling {RequestName} [{RequestId}]",
                requestName,
                requestId);

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var response = await next();
                stopwatch.Stop();

                _logger.LogInformation(
                    "Completed {RequestName} [{RequestId}] in {ElapsedMilliseconds}ms",
                    requestName,
                    requestId,
                    stopwatch.ElapsedMilliseconds);

                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                _logger.LogError(
                    ex,
                    "Exception occurred in {RequestName} [{RequestId}] after {ElapsedMilliseconds}ms",
                    requestName,
                    requestId,
                    stopwatch.ElapsedMilliseconds);

                throw;
            }
        }
    }
}
using FluentValidation;
using Inventra.Application.Common.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Inventra.Application.Common.Behaviors
{
    /// <summary>
    /// MediatR pipeline behavior for validating requests using FluentValidation.
    /// Executes before the handler to catch validation errors early.
    /// </summary>
    /// <typeparam name="TRequest">Type of request being validated.</typeparam>
    /// <typeparam name="TResponse">Type of response from the handler.</typeparam>
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

        public ValidationBehavior(
            IEnumerable<IValidator<TRequest>> validators,
            ILogger<ValidationBehavior<TRequest, TResponse>> logger)
        {
            _validators = validators;
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .Where(r => !r.IsValid)
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Count == 0)
                return await next();

            var errorMessages = failures
                .Select(f => $"{f.PropertyName}: {f.ErrorMessage}")
                .ToList();

            _logger.LogWarning(
                "Validation failed for {RequestName}. Errors: {Errors}",
                typeof(TRequest).Name,
                string.Join("; ", errorMessages));

            // Handle both Result and Result<TData> responses
            var resultType = typeof(TResponse);
            if (resultType.IsGenericType && resultType.GetGenericTypeDefinition().Name == "Result`1")
            {
                var dataType = resultType.GetGenericArguments()[0];
                var failureMethod = typeof(Result)
                    .GetMethod(nameof(Result.Failure), new[] { typeof(IEnumerable<string>) })!
                    .MakeGenericMethod(dataType);
                return (TResponse)failureMethod.Invoke(null, new object[] { errorMessages })!;
            }
            else
            {
                return (TResponse)(object)Result.Failure(errorMessages);
            }
        }
    }
}
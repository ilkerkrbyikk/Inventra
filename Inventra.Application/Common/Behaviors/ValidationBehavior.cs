using FluentValidation;
using Inventra.Application.Common.Results;
using MediatR;

namespace Inventra.Application.Common.CQRS
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .Distinct()
                .ToList();

            if (failures.Any())
            {
                var errors = failures.Select(f => $"{f.PropertyName}: {f.ErrorMessage}").ToList();

                // Create a Result or Result<T> failure response
                if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
                {
                    var resultType = typeof(Result<>).MakeGenericType(typeof(TResponse).GetGenericArguments()[0]);
                    var failureMethod = resultType.GetMethod("Failure", new[] { typeof(IEnumerable<string>) });
                    var failureResult = failureMethod?.Invoke(null, new object[] { errors });
                    return failureResult as TResponse ?? throw new InvalidOperationException("Failed to create failure result.");
                }
                else if (typeof(TResponse) == typeof(Result))
                {
                    return Result.Failure(errors) as TResponse ?? throw new InvalidOperationException("Failed to create failure result.");
                }
            }

            return await next();
        }
    }
}
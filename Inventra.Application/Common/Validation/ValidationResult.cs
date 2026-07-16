using FluentValidation.Results;

namespace Inventra.Application.Common.Validation
{
    public class ValidationFailure
    {
        public string PropertyName { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }

    public static class ValidationExtensions
    {
        public static IEnumerable<string> GetErrorMessages(this ValidationResult validationResult)
            => validationResult.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}");
    }
}
using FluentValidation;

namespace Inventra.Application.Features.Products.Queries
{
    /// <summary>
    /// Validator for GetAllProductsQuery.
    /// Ensures pagination parameters are valid.
    /// </summary>
    public class GetAllProductsQueryValidator : AbstractValidator<GetAllProductsQuery>
    {
        public GetAllProductsQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Page number must be at least 1.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .LessThanOrEqualTo(1000)
                .WithMessage("Page size must be between 1 and 1000.");
        }
    }
}
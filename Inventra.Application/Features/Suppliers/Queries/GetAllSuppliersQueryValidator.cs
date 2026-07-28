using FluentValidation;

namespace Inventra.Application.Features.Suppliers.Queries
{
    /// <summary>
    /// Validator for GetAllSuppliersQuery.
    /// </summary>
    public class GetAllSuppliersQueryValidator : AbstractValidator<GetAllSuppliersQuery>
    {
        public GetAllSuppliersQueryValidator()
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
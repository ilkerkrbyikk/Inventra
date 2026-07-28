using FluentValidation;

namespace Inventra.Application.Features.Branches.Queries
{
    /// <summary>
    /// Validator for GetAllBranchesQuery.
    /// Ensures pagination parameters are valid.
    /// </summary>
    public class GetAllBranchesQueryValidator : AbstractValidator<GetAllBranchesQuery>
    {
        public GetAllBranchesQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Page number must be greater than or equal to 1.");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(1000)
                .WithMessage("Page size must be between 1 and 1000.");
        }
    }
}

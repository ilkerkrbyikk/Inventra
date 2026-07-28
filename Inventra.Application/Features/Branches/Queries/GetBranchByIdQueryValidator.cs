using FluentValidation;

namespace Inventra.Application.Features.Branches.Queries
{
    /// <summary>
    /// Validator for GetBranchByIdQuery.
    /// Ensures the branch ID is valid.
    /// </summary>
    public class GetBranchByIdQueryValidator : AbstractValidator<GetBranchByIdQuery>
    {
        public GetBranchByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Branch ID cannot be empty.");
        }
    }
}

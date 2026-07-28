using FluentValidation;

namespace Inventra.Application.Features.Branches.Commands
{
    /// <summary>
    /// Validator for DeleteBranchCommand.
    /// Ensures the branch ID is valid.
    /// </summary>
    public class DeleteBranchCommandValidator : AbstractValidator<DeleteBranchCommand>
    {
        public DeleteBranchCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Branch ID cannot be empty.");
        }
    }
}

using FluentValidation;

namespace Inventra.Application.Features.Branches.Commands
{
    /// <summary>
    /// Validator for UpdateBranchCommand.
    /// Ensures the branch ID is valid and optional fields meet requirements if provided.
    /// </summary>
    public class UpdateBranchCommandValidator : AbstractValidator<UpdateBranchCommand>
    {
        public UpdateBranchCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Branch ID cannot be empty.");

            RuleFor(x => x.Name)
                .MaximumLength(255)
                .WithMessage("Branch name cannot exceed 255 characters.")
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.Address)
                .MaximumLength(500)
                .WithMessage("Branch address cannot exceed 500 characters.")
                .When(x => !string.IsNullOrEmpty(x.Address));
        }
    }
}

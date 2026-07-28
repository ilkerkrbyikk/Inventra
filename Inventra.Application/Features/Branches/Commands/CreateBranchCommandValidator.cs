using FluentValidation;

namespace Inventra.Application.Features.Branches.Commands
{
    /// <summary>
    /// Validator for CreateBranchCommand.
    /// Ensures all branch creation parameters are valid.
    /// </summary>
    public class CreateBranchCommandValidator : AbstractValidator<CreateBranchCommand>
    {
        public CreateBranchCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Branch name is required.")
                .MaximumLength(255)
                .WithMessage("Branch name cannot exceed 255 characters.");

            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage("Branch address is required.")
                .MaximumLength(500)
                .WithMessage("Branch address cannot exceed 500 characters.");
        }
    }
}

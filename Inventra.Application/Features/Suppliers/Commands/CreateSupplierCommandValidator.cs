using FluentValidation;

namespace Inventra.Application.Features.Suppliers.Commands
{
    /// <summary>
    /// Validator for CreateSupplierCommand.
    /// Ensures all supplier creation parameters are valid.
    /// </summary>
    public class CreateSupplierCommandValidator : AbstractValidator<CreateSupplierCommand>
    {
        public CreateSupplierCommandValidator()
        {
            RuleFor(x => x.FirmName)
                .NotEmpty()
                .WithMessage("Firm name is required.")
                .MaximumLength(255)
                .WithMessage("Firm name cannot exceed 255 characters.");

            RuleFor(x => x.ContactInfo)
                .NotEmpty()
                .WithMessage("Contact info is required.")
                .MaximumLength(255)
                .WithMessage("Contact info cannot exceed 255 characters.");

            RuleFor(x => x.AuthorizedPerson)
                .NotEmpty()
                .WithMessage("Authorized person name is required.")
                .MaximumLength(255)
                .WithMessage("Authorized person name cannot exceed 255 characters.");
        }
    }
}

using FluentValidation;

namespace Inventra.Application.Features.Suppliers.Commands
{
    /// <summary>
    /// Validator for UpdateSupplierCommand.
    /// Ensures the supplier ID is valid and optional fields meet requirements if provided.
    /// </summary>
    public class UpdateSupplierCommandValidator : AbstractValidator<UpdateSupplierCommand>
    {
        public UpdateSupplierCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Supplier ID cannot be empty.");

            RuleFor(x => x.FirmName)
                .MaximumLength(255)
                .WithMessage("Firm name cannot exceed 255 characters.")
                .When(x => !string.IsNullOrEmpty(x.FirmName));

            RuleFor(x => x.ContactInfo)
                .MaximumLength(255)
                .WithMessage("Contact info cannot exceed 255 characters.")
                .When(x => !string.IsNullOrEmpty(x.ContactInfo));

            RuleFor(x => x.AuthorizedPerson)
                .MaximumLength(255)
                .WithMessage("Authorized person name cannot exceed 255 characters.")
                .When(x => !string.IsNullOrEmpty(x.AuthorizedPerson));
        }
    }
}

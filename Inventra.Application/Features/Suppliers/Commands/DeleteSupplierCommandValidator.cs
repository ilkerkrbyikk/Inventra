using FluentValidation;

namespace Inventra.Application.Features.Suppliers.Commands
{
    /// <summary>
    /// Validator for DeleteSupplierCommand.
    /// Ensures the supplier ID is valid.
    /// </summary>
    public class DeleteSupplierCommandValidator : AbstractValidator<DeleteSupplierCommand>
    {
        public DeleteSupplierCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Supplier ID cannot be empty.");
        }
    }
}

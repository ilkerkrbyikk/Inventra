using FluentValidation;

namespace Inventra.Application.Features.Warehouses.Commands
{
    /// <summary>
    /// Validator for UpdateWarehouseCommand.
    /// Ensures the warehouse ID is valid and optional fields meet requirements if provided.
    /// </summary>
    public class UpdateWarehouseCommandValidator : AbstractValidator<UpdateWarehouseCommand>
    {
        public UpdateWarehouseCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Warehouse ID cannot be empty.");

            RuleFor(x => x.Name)
                .MaximumLength(255)
                .WithMessage("Warehouse name cannot exceed 255 characters.")
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.Address)
                .MaximumLength(500)
                .WithMessage("Warehouse address cannot exceed 500 characters.")
                .When(x => !string.IsNullOrEmpty(x.Address));
        }
    }
}

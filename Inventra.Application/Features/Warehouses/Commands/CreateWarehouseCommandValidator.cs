using FluentValidation;

namespace Inventra.Application.Features.Warehouses.Commands
{
    /// <summary>
    /// Validator for CreateWarehouseCommand.
    /// Ensures all warehouse creation parameters are valid.
    /// </summary>
    public class CreateWarehouseCommandValidator : AbstractValidator<CreateWarehouseCommand>
    {
        public CreateWarehouseCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Warehouse name is required.")
                .MaximumLength(255)
                .WithMessage("Warehouse name cannot exceed 255 characters.");

            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage("Warehouse address is required.")
                .MaximumLength(500)
                .WithMessage("Warehouse address cannot exceed 500 characters.");
        }
    }
}

using FluentValidation;

namespace Inventra.Application.Features.StockTransfer.Commands
{
    /// <summary>
    /// Validator for CreateTransferRequestCommand.
    /// Ensures all input parameters are valid before handler execution.
    /// </summary>
    public class CreateTransferRequestCommandValidator : AbstractValidator<CreateTransferRequestCommand>
    {
        public CreateTransferRequestCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage("Product ID cannot be empty.");

            RuleFor(x => x.FromWarehouseId)
                .NotEqual(Guid.Empty)
                .WithMessage("Source warehouse ID cannot be empty.");

            RuleFor(x => x.ToWarehouseId)
                .NotEqual(Guid.Empty)
                .WithMessage("Destination warehouse ID cannot be empty.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than zero.");

            RuleFor(x => x)
                .Must(x => x.FromWarehouseId != x.ToWarehouseId)
                .WithMessage("Source and destination warehouses must be different.");
        }
    }
}
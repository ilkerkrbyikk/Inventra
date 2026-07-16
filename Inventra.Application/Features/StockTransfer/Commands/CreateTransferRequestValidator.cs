using FluentValidation;
using Inventra.Application.Features.StockTransfer.Commands;

namespace Inventra.Application.Features.StockTransfer.Validators
{
    public class CreateTransferRequestCommandValidator : AbstractValidator<CreateTransferRequestCommand>
    {
        public CreateTransferRequestCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product ID is required.");

            RuleFor(x => x.FromWarehouseId)
                .NotEmpty().WithMessage("Source warehouse ID is required.");

            RuleFor(x => x.ToWarehouseId)
                .NotEmpty().WithMessage("Destination warehouse ID is required.")
                .NotEqual(x => x.FromWarehouseId).WithMessage("Source and destination warehouses cannot be the same.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        }
    }
}
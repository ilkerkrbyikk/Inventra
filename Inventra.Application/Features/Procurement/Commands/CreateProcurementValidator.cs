using FluentValidation;
using Inventra.Application.Features.Procurement.Commands;

namespace Inventra.Application.Features.Procurement.Validators
{
    public class CreateProcurementCommandValidator : AbstractValidator<CreateProcurementCommand>
    {
        public CreateProcurementCommandValidator()
        {
            RuleFor(x => x.SupplierId)
                .NotEmpty().WithMessage("Supplier ID is required.");

            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product ID is required.");

            RuleFor(x => x.WarehouseId)
                .NotEmpty().WithMessage("Warehouse ID is required.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than zero.");
        }
    }
}
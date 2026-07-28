using FluentValidation;

namespace Inventra.Application.Features.BranchInventoryRequests.Commands
{
    /// <summary>
    /// Validator for CreateBranchInventoryRequestCommand.
    /// Ensures all required fields are present and values are within acceptable ranges.
    /// Executed automatically by the ValidationBehavior MediatR pipeline.
    /// </summary>
    public class CreateBranchInventoryRequestCommandValidator
        : AbstractValidator<CreateBranchInventoryRequestCommand>
    {
        public CreateBranchInventoryRequestCommandValidator()
        {
            RuleFor(x => x.BranchId)
                .NotEqual(Guid.Empty)
                .WithMessage("Branch ID cannot be empty.");

            RuleFor(x => x.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage("Product ID cannot be empty.");

            RuleFor(x => x.WarehouseId)
                .NotEqual(Guid.Empty)
                .WithMessage("Warehouse ID cannot be empty.");

            RuleFor(x => x.RequestedQuantity)
                .GreaterThan(0)
                .WithMessage("Requested quantity must be greater than zero.");

            RuleFor(x => x.Notes)
                .MaximumLength(500)
                .WithMessage("Notes cannot exceed 500 characters.")
                .When(x => x.Notes is not null);

            RuleFor(x => x.WarehouseManagerUserId)
                .NotEmpty()
                .WithMessage("Warehouse manager user ID is required for notification routing.");
        }
    }
}

using FluentValidation;

namespace Inventra.Application.Features.Products.Commands
{
    /// <summary>
    /// Validator for UpdateProductCommand.
    /// Ensures the product ID is valid and optional fields meet requirements if provided.
    /// </summary>
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Product ID cannot be empty.");

            RuleFor(x => x.Name)
                .MaximumLength(255)
                .WithMessage("Product name cannot exceed 255 characters.")
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greater than zero.")
                .When(x => x.Price.HasValue);

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Stock quantity cannot be negative.")
                .When(x => x.StockQuantity.HasValue);
        }
    }
}
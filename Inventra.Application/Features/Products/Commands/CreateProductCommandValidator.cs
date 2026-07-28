using FluentValidation;

namespace Inventra.Application.Features.Products.Commands
{
    /// <summary>
    /// Validator for CreateProductCommand.
    /// Ensures all product creation parameters are valid.
    /// </summary>
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Product name is required.")
                .MaximumLength(255)
                .WithMessage("Product name cannot exceed 255 characters.");

            RuleFor(x => x.Barcode)
                .NotEmpty()
                .WithMessage("Barcode is required.")
                .MaximumLength(100)
                .WithMessage("Barcode cannot exceed 100 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greater than zero.");

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Stock quantity cannot be negative.");

            RuleFor(x => x.CriticalStockThreshold)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Critical stock threshold cannot be negative.")
                .When(x => x.CriticalStockThreshold.HasValue);
        }
    }
}

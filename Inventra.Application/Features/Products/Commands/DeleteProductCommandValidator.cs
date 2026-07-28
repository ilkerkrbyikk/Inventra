using FluentValidation;

namespace Inventra.Application.Features.Products.Commands
{
    /// <summary>
    /// Validator for DeleteProductCommand.
    /// Ensures the product ID is valid.
    /// </summary>
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Product ID cannot be empty.");
        }
    }
}
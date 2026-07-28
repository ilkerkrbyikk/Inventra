using FluentValidation;

namespace Inventra.Application.Features.Products.Queries
{
    /// <summary>
    /// Validator for GetProductByIdQuery.
    /// Ensures the product ID is not empty.
    /// </summary>
    public class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
    {
        public GetProductByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Product ID cannot be empty.");
        }
    }
}
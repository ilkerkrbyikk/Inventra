using FluentValidation;

namespace Inventra.Application.Features.Suppliers.Queries
{
    /// <summary>
    /// Validator for GetSupplierByIdQuery.
    /// </summary>
    public class GetSupplierByIdQueryValidator : AbstractValidator<GetSupplierByIdQuery>
    {
        public GetSupplierByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Supplier ID cannot be empty.");
        }
    }
}
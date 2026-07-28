using Inventra.Application.Common.CQRS;
using Inventra.Application.Common.Results;
using Inventra.Application.Interfaces;
using Inventra.Domain.Entities;

namespace Inventra.Application.Features.Suppliers.Commands
{
    /// <summary>
    /// Handler for CreateSupplierCommand.
    /// Creates a new supplier and returns its ID.
    /// Validation is performed by the ValidationBehavior pipeline.
    /// </summary>
    public class CreateSupplierCommandHandler : ICommandHandler<CreateSupplierCommand, Guid>
    {
        private readonly IGenericRepository<Supplier> _supplierRepository;

        public CreateSupplierCommandHandler(IGenericRepository<Supplier> supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<Result<Guid>> Handle(
            CreateSupplierCommand request,
            CancellationToken cancellationToken)
        {
            // Validation already done by ValidationBehavior
            // Create new supplier
            var supplier = new Supplier
            {
                Id = Guid.NewGuid(),
                FirmName = request.FirmName,
                ContactInfo = request.ContactInfo,
                AuthorizedPerson = request.AuthorizedPerson,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            await _supplierRepository.AddAsync(supplier, cancellationToken);

            return Result.Success(supplier.Id, "Supplier created successfully.");
        }
    }
}

using Inventra.Application.Common.CQRS;
using Inventra.Application.Common.Results;
using Inventra.Application.Interfaces;
using Inventra.Domain.Entities;

namespace Inventra.Application.Features.Suppliers.Commands
{
    /// <summary>
    /// Handler for UpdateSupplierCommand.
    /// Updates an existing supplier with provided fields only (null fields are ignored).
    /// Validation is performed by the ValidationBehavior pipeline.
    /// </summary>
    public class UpdateSupplierCommandHandler : ICommandHandler<UpdateSupplierCommand>
    {
        private readonly IGenericRepository<Supplier> _supplierRepository;

        public UpdateSupplierCommandHandler(IGenericRepository<Supplier> supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<Result> Handle(
            UpdateSupplierCommand request,
            CancellationToken cancellationToken)
        {
            // Validation already done by ValidationBehavior
            var supplier = await _supplierRepository.GetByIdAsync(request.Id, cancellationToken);

            if (supplier is null)
                return Result.Failure("Supplier not found.");

            // Update only provided fields
            if (!string.IsNullOrEmpty(request.FirmName))
                supplier.FirmName = request.FirmName;

            if (!string.IsNullOrEmpty(request.ContactInfo))
                supplier.ContactInfo = request.ContactInfo;

            if (!string.IsNullOrEmpty(request.AuthorizedPerson))
                supplier.AuthorizedPerson = request.AuthorizedPerson;

            supplier.UpdatedAt = DateTime.UtcNow;

            await _supplierRepository.UpdateAsync(supplier, cancellationToken);

            return Result.Success("Supplier updated successfully.");
        }
    }
}

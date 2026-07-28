using Inventra.Application.Common.CQRS;
using Inventra.Application.Common.Results;
using Inventra.Application.Interfaces;
using Inventra.Domain.Entities;

namespace Inventra.Application.Features.Suppliers.Commands
{
    /// <summary>
    /// Handler for DeleteSupplierCommand.
    /// Performs a soft delete by marking the supplier as deleted.
    /// Validation is performed by the ValidationBehavior pipeline.
    /// </summary>
    public class DeleteSupplierCommandHandler : ICommandHandler<DeleteSupplierCommand>
    {
        private readonly IGenericRepository<Supplier> _supplierRepository;

        public DeleteSupplierCommandHandler(IGenericRepository<Supplier> supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<Result> Handle(
            DeleteSupplierCommand request,
            CancellationToken cancellationToken)
        {
            // Validation already done by ValidationBehavior
            var supplier = await _supplierRepository.GetByIdAsync(request.Id, cancellationToken);

            if (supplier is null)
                return Result.Failure("Supplier not found.");

            // Soft delete
            supplier.IsDeleted = true;
            supplier.DeletedAt = DateTime.UtcNow;
            supplier.UpdatedAt = DateTime.UtcNow;

            await _supplierRepository.UpdateAsync(supplier, cancellationToken);

            return Result.Success("Supplier deleted successfully.");
        }
    }
}

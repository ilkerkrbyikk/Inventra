using Inventra.Application.Common.CQRS;
using Inventra.Application.Common.Results;
using Inventra.Application.Features.Suppliers.DTOs;
using Inventra.Application.Interfaces;
using Inventra.Domain.Entities;

namespace Inventra.Application.Features.Suppliers.Queries
{
    /// <summary>
    /// Handler for GetSupplierByIdQuery.
    /// Retrieves a supplier by ID and maps it to SupplierDto.
    /// </summary>
    public class GetSupplierByIdQueryHandler : IQueryHandler<GetSupplierByIdQuery, SupplierDto>
    {
        private readonly IGenericRepository<Supplier> _supplierRepository;

        public GetSupplierByIdQueryHandler(IGenericRepository<Supplier> supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<Result<SupplierDto>> Handle(
            GetSupplierByIdQuery request,
            CancellationToken cancellationToken)
        {
            var supplier = await _supplierRepository.GetByIdAsync(request.Id, cancellationToken);

            if (supplier is null)
                return Result.Failure<SupplierDto>("Supplier not found.");

            var supplierDto = new SupplierDto(
                supplier.Id,
                supplier.FirmName,
                supplier.ContactInfo,
                supplier.AuthorizedPerson,
                supplier.CreatedAt,
                supplier.UpdatedAt);

            return Result.Success(supplierDto, "Supplier retrieved successfully.");
        }
    }
}
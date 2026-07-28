using Inventra.Application.Common.CQRS;
using Inventra.Application.Common.Results;
using Inventra.Application.Features.Suppliers.DTOs;
using Inventra.Application.Interfaces;
using Inventra.Domain.Entities;

namespace Inventra.Application.Features.Suppliers.Queries
{
    /// <summary>
    /// Handler for GetAllSuppliersQuery.
    /// </summary>
    public class GetAllSuppliersQueryHandler : IQueryHandler<GetAllSuppliersQuery, IEnumerable<SupplierDto>>
    {
        private readonly IGenericRepository<Supplier> _supplierRepository;

        public GetAllSuppliersQueryHandler(IGenericRepository<Supplier> supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<Result<IEnumerable<SupplierDto>>> Handle(
            GetAllSuppliersQuery request,
            CancellationToken cancellationToken)
        {
            var suppliers = await _supplierRepository.GetAllAsync(cancellationToken);

            var supplierDtos = suppliers
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(s => new SupplierDto(
                    s.Id,
                    s.FirmName,
                    s.ContactInfo,
                    s.AuthorizedPerson,
                    s.CreatedAt,
                    s.UpdatedAt))
                .ToList();

            return Result.Success<IEnumerable<SupplierDto>>(
                supplierDtos,
                $"Retrieved {supplierDtos.Count} suppliers.");
        }
    }
}
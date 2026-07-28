using Inventra.Application.Common.CQRS;
using Inventra.Application.Features.Suppliers.DTOs;

namespace Inventra.Application.Features.Suppliers.Queries
{
    /// <summary>
    /// Query to retrieve all suppliers.
    /// </summary>
    public record GetAllSuppliersQuery(int PageNumber = 1, int PageSize = 100) : IQuery<IEnumerable<SupplierDto>>;
}
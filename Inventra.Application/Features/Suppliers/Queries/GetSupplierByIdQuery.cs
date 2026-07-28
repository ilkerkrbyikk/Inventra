using Inventra.Application.Common.CQRS;
using Inventra.Application.Features.Suppliers.DTOs;

namespace Inventra.Application.Features.Suppliers.Queries
{
    /// <summary>
    /// Query to retrieve a supplier by its ID.
    /// </summary>
    public record GetSupplierByIdQuery(Guid Id) : IQuery<SupplierDto>;
}
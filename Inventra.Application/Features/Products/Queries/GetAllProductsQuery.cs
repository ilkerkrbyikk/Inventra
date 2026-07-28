using Inventra.Application.Common.CQRS;
using Inventra.Application.Features.Products.DTOs;

namespace Inventra.Application.Features.Products.Queries
{
    /// <summary>
    /// Query to retrieve all products with pagination support.
    /// </summary>
    public record GetAllProductsQuery(int PageNumber = 1, int PageSize = 100) : IQuery<IEnumerable<ProductDto>>;
}
using Inventra.Application.Common.CQRS;
using Inventra.Application.Features.Products.DTOs;

namespace Inventra.Application.Features.Products.Queries
{
    /// <summary>
    /// Query to retrieve a product by its ID.
    /// Returns ProductDto with all product details.
    /// </summary>
    public record GetProductByIdQuery(Guid Id) : IQuery<ProductDto>;
}
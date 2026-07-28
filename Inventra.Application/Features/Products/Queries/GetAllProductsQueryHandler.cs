using Inventra.Application.Common.CQRS;
using Inventra.Application.Common.Results;
using Inventra.Application.Features.Products.DTOs;
using Inventra.Application.Interfaces;
using Inventra.Domain.Entities;

namespace Inventra.Application.Features.Products.Queries
{
    /// <summary>
    /// Handler for GetAllProductsQuery.
    /// Retrieves all products with pagination and maps them to ProductDto list.
    /// Validation is performed by the ValidationBehavior pipeline.
    /// </summary>
    public class GetAllProductsQueryHandler : IQueryHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IGenericRepository<Product> _productRepository;

        public GetAllProductsQueryHandler(IGenericRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<IEnumerable<ProductDto>>> Handle(
            GetAllProductsQuery request,
            CancellationToken cancellationToken)
        {
            // Validation already done by ValidationBehavior
            var products = await _productRepository.GetAllAsync(cancellationToken);

            var productDtos = products
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new ProductDto(
                    p.Id,
                    p.Name,
                    p.Barcode,
                    p.Price,
                    p.StockQuantity,
                    p.CreatedAt,
                    p.UpdatedAt))
                .ToList();

            return Result.Success<IEnumerable<ProductDto>>(
                productDtos,
                $"Retrieved {productDtos.Count} products.");
        }
    }
}
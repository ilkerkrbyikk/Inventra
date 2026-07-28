using Inventra.Application.Common.CQRS;
using Inventra.Application.Common.Results;
using Inventra.Application.Features.Products.DTOs;
using Inventra.Application.Interfaces;
using Inventra.Domain.Entities;

namespace Inventra.Application.Features.Products.Queries
{
    /// <summary>
    /// Handler for GetProductByIdQuery.
    /// Retrieves a product by ID and maps it to ProductDto.
    /// Validation is performed by the ValidationBehavior pipeline.
    /// </summary>
    public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly IGenericRepository<Product> _productRepository;

        public GetProductByIdQueryHandler(IGenericRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<ProductDto>> Handle(
            GetProductByIdQuery request,
            CancellationToken cancellationToken)
        {
            // Validation already done by ValidationBehavior
            var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);

            if (product is null)
                return Result.Failure<ProductDto>("Product not found.");

            var productDto = new ProductDto(
                product.Id,
                product.Name,
                product.Barcode,
                product.Price,
                product.StockQuantity,
                product.CreatedAt,
                product.UpdatedAt);

            return Result.Success(productDto, "Product retrieved successfully.");
        }
    }
}
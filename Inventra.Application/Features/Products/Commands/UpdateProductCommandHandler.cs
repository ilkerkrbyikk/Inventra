using Inventra.Application.Common.CQRS;
using Inventra.Application.Common.Results;
using Inventra.Application.Interfaces;
using Inventra.Domain.Entities;

namespace Inventra.Application.Features.Products.Commands
{
    /// <summary>
    /// Handler for UpdateProductCommand.
    /// Updates an existing product with provided fields only (null fields are ignored).
    /// Validation is performed by the ValidationBehavior pipeline.
    /// </summary>
    public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
    {
        private readonly IGenericRepository<Product> _productRepository;

        public UpdateProductCommandHandler(IGenericRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result> Handle(
            UpdateProductCommand request,
            CancellationToken cancellationToken)
        {
            // Validation already done by ValidationBehavior
            var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);

            if (product is null)
                return Result.Failure("Product not found.");

            // Update only provided fields
            if (!string.IsNullOrEmpty(request.Name))
                product.Name = request.Name;

            if (request.Price.HasValue)
                product.Price = request.Price.Value;

            if (request.StockQuantity.HasValue)
                product.StockQuantity = request.StockQuantity.Value;

            if (request.CriticalStockThreshold.HasValue)
                product.CriticalStockThreshold = request.CriticalStockThreshold.Value;

            product.UpdatedAt = DateTime.UtcNow;

            await _productRepository.UpdateAsync(product, cancellationToken);

            return Result.Success("Product updated successfully.");
        }
    }
}

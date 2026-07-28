using Inventra.Application.Common.CQRS;
using Inventra.Application.Common.Results;
using Inventra.Application.Interfaces;
using Inventra.Domain.Entities;

namespace Inventra.Application.Features.Products.Commands
{
    /// <summary>
    /// Handler for CreateProductCommand.
    /// Creates a new product and returns its ID.
    /// Validation is performed by the ValidationBehavior pipeline.
    /// </summary>
    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Guid>
    {
        private readonly IGenericRepository<Product> _productRepository;

        public CreateProductCommandHandler(IGenericRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<Guid>> Handle(
            CreateProductCommand request,
            CancellationToken cancellationToken)
        {
            // Validation already done by ValidationBehavior
            // Check if product with same barcode already exists
            var existingProducts = await _productRepository.GetAllAsync(cancellationToken);
            if (existingProducts.Any(p => p.Barcode == request.Barcode))
                return Result.Failure<Guid>("A product with this barcode already exists.");

            // Create new product
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Barcode = request.Barcode,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            await _productRepository.AddAsync(product, cancellationToken);

            return Result.Success(product.Id, "Product created successfully.");
        }
    }
}
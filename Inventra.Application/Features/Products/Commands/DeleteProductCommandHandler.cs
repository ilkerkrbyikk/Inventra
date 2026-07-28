using Inventra.Application.Common.CQRS;
using Inventra.Application.Common.Results;
using Inventra.Application.Interfaces;
using Inventra.Domain.Entities;

namespace Inventra.Application.Features.Products.Commands
{
    /// <summary>
    /// Handler for DeleteProductCommand.
    /// Performs a soft delete by marking the product as deleted.
    /// Validation is performed by the ValidationBehavior pipeline.
    /// </summary>
    public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
    {
        private readonly IGenericRepository<Product> _productRepository;

        public DeleteProductCommandHandler(IGenericRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result> Handle(
            DeleteProductCommand request,
            CancellationToken cancellationToken)
        {
            // Validation already done by ValidationBehavior
            var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);

            if (product is null)
                return Result.Failure("Product not found.");

            // Soft delete
            product.IsDeleted = true;
            product.DeletedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;

            await _productRepository.UpdateAsync(product, cancellationToken);

            return Result.Success("Product deleted successfully.");
        }
    }
}
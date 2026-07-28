using Inventra.Application.Common.CQRS;
using Inventra.Application.Common.Notifications;
using Inventra.Application.Common.Results;
using Inventra.Application.Interfaces;
using Inventra.Domain.Entities;
using MediatR;

namespace Inventra.Application.Features.StockTransfer.Commands
{
    /// <summary>
    /// Handler for StartTransferCommand.
    /// Transitions a transfer request from Pending to InTransit status.
    /// Updates the product's stock quantity accordingly.
    /// Validation is performed by the ValidationBehavior pipeline before this handler executes.
    /// </summary>
    public class StartTransferCommandHandler : ICommandHandler<StartTransferCommand>
    {
        private readonly IGenericRepository<StockTransaction> _transactionRepository;
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IPublisher _publisher;

        public StartTransferCommandHandler(
            IGenericRepository<StockTransaction> transactionRepository,
            IGenericRepository<Product> productRepository,
            IPublisher publisher)
        {
            _transactionRepository = transactionRepository;
            _productRepository = productRepository;
            _publisher = publisher;
        }

        public async Task<Result> Handle(
            StartTransferCommand request,
            CancellationToken cancellationToken)
        {
            // Validation is already done by ValidationBehavior
            // Get transaction
            var transaction = await _transactionRepository.GetByIdAsync(request.TransactionId, cancellationToken);
            if (transaction is null)
                return Result.Failure("Transfer request not found.");

            // Check if status is Pending
            if (transaction.Status != "Pending")
                return Result.Failure("Only pending transfers can be started.");

            // Get product to verify stock
            var product = await _productRepository.GetByIdAsync(transaction.ProductId, cancellationToken);
            if (product is null)
                return Result.Failure("Product not found.");

            if (product.StockQuantity < transaction.RequestedQuantity)
                return Result.Failure("Insufficient stock to complete transfer.");

            // Update stock and transaction status
            var stockQuantityBeforeTransfer = product.StockQuantity;
            product.StockQuantity -= transaction.RequestedQuantity;
            transaction.Status = "InTransit";
            transaction.UpdatedAt = DateTime.UtcNow;

            await _productRepository.UpdateAsync(product, cancellationToken);
            await _transactionRepository.UpdateAsync(transaction, cancellationToken);

            if (product.CriticalStockThreshold.HasValue &&
                stockQuantityBeforeTransfer > product.CriticalStockThreshold.Value &&
                product.StockQuantity <= product.CriticalStockThreshold.Value)
            {
                await _publisher.Publish(
                    new LowStockDetectedNotification(
                        product.Id,
                        product.Name,
                        product.StockQuantity,
                        product.CriticalStockThreshold.Value,
                        string.Empty),
                    cancellationToken);
            }

            return Result.Success("Transfer started successfully.");
        }
    }
}

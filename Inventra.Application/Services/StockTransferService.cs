using Inventra.Application.DTOs;
using Inventra.Application.Interfaces;
using Inventra.Domain.Entities;

namespace Inventra.Application.Services
{
    public class StockTransferService
    {
        private readonly IStockTransactionRepository _transactionRepository;
        private readonly IProductRepository _productRepository;

        public StockTransferService(IStockTransactionRepository transactionRepository, IProductRepository productRepository)
        {
            _transactionRepository = transactionRepository;
            _productRepository = productRepository;
        }

        public async Task<StockTransaction> CreateTransferRequestAsync(StockTransferRequestDto dto)
        {
            var product = await _productRepository.GetByIdAsync(dto.ProductId);
            if (product == null)
                throw new ArgumentException("Product not found.");

            if (product.StockQuantity < dto.Quantity)
                throw new InvalidOperationException("Insufficient stock in source warehouse.");

            var transaction = new StockTransaction
            {
                Id = Guid.NewGuid(),
                ProductId = dto.ProductId,
                FromWarehouseId = dto.FromWarehouseId,
                ToWarehouseId = dto.ToWarehouseId,
                RequestedQuantity = dto.Quantity,
                TransferredQuantity = 0,
                DefectiveQuantity = 0,
                TransactionDate = DateTime.UtcNow,
                Status = "Pending"
            };

            await _transactionRepository.AddAsync(transaction);
            await _transactionRepository.SaveChangesAsync();

            return transaction;
        }

        public async Task StartTransferAsync(Guid transactionId)
        {
            var transaction = await _transactionRepository.GetByIdAsync(transactionId);
            if (transaction == null)
                throw new ArgumentException("Transaction not found.");

            if (transaction.Status != "Pending")
                throw new InvalidOperationException("Only pending transfers can be started.");

            var product = await _productRepository.GetByIdAsync(transaction.ProductId);
            if (product == null)
                throw new ArgumentException("Product not found.");

            product.StockQuantity -= transaction.RequestedQuantity;
            transaction.Status = "InTransit";
            transaction.UpdatedAt = DateTime.UtcNow;

            await _productRepository.UpdateAsync(product);
            await _transactionRepository.UpdateAsync(transaction);
            await _transactionRepository.SaveChangesAsync();
        }

        public async Task CompleteTransferAsync(StockTransferCompleteDto dto)
        {
            var transaction = await _transactionRepository.GetByIdAsync(dto.TransactionId);
            if (transaction == null)
                throw new ArgumentException("Transaction not found.");

            if (transaction.Status != "InTransit")
                throw new InvalidOperationException("Only in-transit transfers can be completed.");

            var product = await _productRepository.GetByIdAsync(transaction.ProductId);
            if (product == null)
                throw new ArgumentException("Product not found.");

            int totalReceived = dto.TransferredQuantity + dto.DefectiveQuantity;
            if (totalReceived != transaction.RequestedQuantity)
                throw new InvalidOperationException("Total quantity mismatch.");

            product.StockQuantity += dto.TransferredQuantity;
            transaction.TransferredQuantity = dto.TransferredQuantity;
            transaction.DefectiveQuantity = dto.DefectiveQuantity;
            transaction.Status = "Completed";
            transaction.Notes = dto.Notes;
            transaction.UpdatedAt = DateTime.UtcNow;

            await _productRepository.UpdateAsync(product);
            await _transactionRepository.UpdateAsync(transaction);
            await _transactionRepository.SaveChangesAsync();
        }
    }
}
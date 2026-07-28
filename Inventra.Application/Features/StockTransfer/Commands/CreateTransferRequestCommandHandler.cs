using Inventra.Application.Common.CQRS;
using Inventra.Application.Common.Results;
using Inventra.Application.Interfaces;
using Inventra.Domain.Entities;

namespace Inventra.Application.Features.StockTransfer.Commands
{
    /// <summary>
    /// Handler for CreateTransferRequestCommand.
    /// Creates a new stock transfer request and returns its ID.
    /// Validation is performed by the ValidationBehavior pipeline before this handler executes.
    /// </summary>
    public class CreateTransferRequestCommandHandler : ICommandHandler<CreateTransferRequestCommand, Guid>
    {
        private readonly IGenericRepository<StockTransaction> _transactionRepository;
        private readonly IGenericRepository<Product> _productRepository;

        public CreateTransferRequestCommandHandler(
            IGenericRepository<StockTransaction> transactionRepository,
            IGenericRepository<Product> productRepository)
        {
            _transactionRepository = transactionRepository;
            _productRepository = productRepository;
        }

        public async Task<Result<Guid>> Handle(
            CreateTransferRequestCommand request,
            CancellationToken cancellationToken)
        {
            // Validation is already done by ValidationBehavior
            // Check if product exists
            var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
            if (product is null)
                return Result.Failure<Guid>("Product not found.");

            // Check stock availability
            if (product.StockQuantity < request.Quantity)
                return Result.Failure<Guid>("Insufficient stock in source warehouse.");

            // Create transfer request
            var transaction = new StockTransaction
            {
                Id = Guid.NewGuid(),
                ProductId = request.ProductId,
                FromWarehouseId = request.FromWarehouseId,
                ToWarehouseId = request.ToWarehouseId,
                RequestedQuantity = request.Quantity,
                TransferredQuantity = 0,
                DefectiveQuantity = 0,
                TransactionDate = DateTime.UtcNow,
                Status = "Pending",
                Notes = string.Empty
            };

            await _transactionRepository.AddAsync(transaction, cancellationToken);

            return Result.Success(transaction.Id, "Transfer request created successfully.");
        }
    }
}
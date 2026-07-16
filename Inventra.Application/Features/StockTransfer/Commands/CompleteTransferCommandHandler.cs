using Inventra.Application.Common.CQRS;
using Inventra.Application.Common.Results;
using Inventra.Application.Common.Validation;
using Inventra.Application.Interfaces;
using FluentValidation;

namespace Inventra.Application.Features.StockTransfer.Commands
{
    public class CompleteTransferCommandHandler : ICommandHandler<CompleteTransferCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CompleteTransferCommand> _validator;

        public CompleteTransferCommandHandler(IUnitOfWork unitOfWork, IValidator<CompleteTransferCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<Result> Handle(CompleteTransferCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                return Result.Failure(validationResult.GetErrorMessages());

            var transaction = await _unitOfWork.StockTransactions.GetByIdAsync(request.TransactionId);
            if (transaction == null)
                return Result.Failure("Transaction not found.");

            if (transaction.Status != "InTransit")
                return Result.Failure("Only in-transit transfers can be completed.");

            int totalReceived = request.TransferredQuantity + request.DefectiveQuantity;
            if (totalReceived != transaction.RequestedQuantity)
                return Result.Failure($"Total quantity mismatch. Expected {transaction.RequestedQuantity}, got {totalReceived}.");

            var product = await _unitOfWork.Products.GetByIdAsync(transaction.ProductId);
            if (product == null)
                return Result.Failure("Product not found.");

            product.StockQuantity += request.TransferredQuantity;
            transaction.TransferredQuantity = request.TransferredQuantity;
            transaction.DefectiveQuantity = request.DefectiveQuantity;
            transaction.Status = "Completed";
            transaction.Notes = request.Notes;
            transaction.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Products.UpdateAsync(product);
            await _unitOfWork.StockTransactions.UpdateAsync(transaction);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success("Transfer completed successfully.");
        }
    }
}
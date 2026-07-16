using Inventra.Application.Common.CQRS;
using Inventra.Application.Common.Results;
using Inventra.Application.Common.Validation;
using Inventra.Application.Interfaces;
using FluentValidation;

namespace Inventra.Application.Features.StockTransfer.Commands
{
    public class StartTransferCommandHandler : ICommandHandler<StartTransferCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<StartTransferCommand> _validator;

        public StartTransferCommandHandler(IUnitOfWork unitOfWork, IValidator<StartTransferCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<Result> Handle(StartTransferCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                return Result.Failure(validationResult.GetErrorMessages());

            var transaction = await _unitOfWork.StockTransactions.GetByIdAsync(request.TransactionId);
            if (transaction == null)
                return Result.Failure("Transaction not found.");

            if (transaction.Status != "Pending")
                return Result.Failure("Only pending transfers can be started.");

            var product = await _unitOfWork.Products.GetByIdAsync(transaction.ProductId);
            if (product == null)
                return Result.Failure("Product not found.");

            product.StockQuantity -= transaction.RequestedQuantity;
            transaction.Status = "InTransit";
            transaction.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Products.UpdateAsync(product);
            await _unitOfWork.StockTransactions.UpdateAsync(transaction);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success("Transfer started successfully.");
        }
    }
}
using Inventra.Application.Common.CQRS;
using Inventra.Application.Common.Results;
using Inventra.Application.Common.Validation;
using Inventra.Application.Interfaces;
using Inventra.Domain.Entities;
using FluentValidation;

namespace Inventra.Application.Features.StockTransfer.Commands
{
    public class CreateTransferRequestCommandHandler : ICommandHandler<CreateTransferRequestCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateTransferRequestCommand> _validator;

        public CreateTransferRequestCommandHandler(IUnitOfWork unitOfWork, IValidator<CreateTransferRequestCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<Result<Guid>> Handle(CreateTransferRequestCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                return Result.Failure<Guid>(validationResult.GetErrorMessages());

            var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId);
            if (product == null)
                return Result.Failure<Guid>("Product not found.");

            if (product.StockQuantity < request.Quantity)
                return Result.Failure<Guid>("Insufficient stock in source warehouse.");

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
                Status = "Pending"
            };

            await _unitOfWork.StockTransactions.AddAsync(transaction);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success(transaction.Id, "Transfer request created successfully.");
        }
    }
}
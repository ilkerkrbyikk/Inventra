using Inventra.Application.Common.CQRS;
using Inventra.Application.Common.Results;
using Inventra.Application.Features.StockTransfer.DTOs;
using Inventra.Application.Interfaces;

namespace Inventra.Application.Features.StockTransfer.Queries
{
    public class GetTransactionByIdQueryHandler : IQueryHandler<GetTransactionByIdQuery, StockTransactionDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTransactionByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<StockTransactionDto>> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            var transaction = await _unitOfWork.StockTransactions.GetByIdAsync(request.TransactionId);
            if (transaction == null)
                return Result.Failure<StockTransactionDto>("Transaction not found.");

            var dto = new StockTransactionDto
            {
                Id = transaction.Id,
                ProductId = transaction.ProductId,
                FromWarehouseId = transaction.FromWarehouseId,
                ToWarehouseId = transaction.ToWarehouseId,
                RequestedQuantity = transaction.RequestedQuantity,
                TransferredQuantity = transaction.TransferredQuantity,
                DefectiveQuantity = transaction.DefectiveQuantity,
                TransactionDate = transaction.TransactionDate,
                Status = transaction.Status,
                Notes = transaction.Notes,
                CreatedAt = transaction.CreatedAt
            };

            return Result.Success(dto);
        }
    }
}
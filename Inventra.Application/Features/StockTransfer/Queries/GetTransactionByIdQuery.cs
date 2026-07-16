using Inventra.Application.Common.CQRS;
using Inventra.Application.Features.StockTransfer.DTOs;

namespace Inventra.Application.Features.StockTransfer.Queries
{
    public class GetTransactionByIdQuery : IQuery<StockTransactionDto>
    {
        public Guid TransactionId { get; set; }
    }
}
using Inventra.Application.Common.CQRS;

namespace Inventra.Application.Features.StockTransfer.Commands
{
    public class StartTransferCommand : ICommand
    {
        public Guid TransactionId { get; set; }
    }
}
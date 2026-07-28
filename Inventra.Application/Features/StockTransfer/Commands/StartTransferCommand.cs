using Inventra.Application.Common.CQRS;

namespace Inventra.Application.Features.StockTransfer.Commands
{
    /// <summary>
    /// Command to start a pending stock transfer.
    /// </summary>
    public record StartTransferCommand(Guid TransactionId) : ICommand;
}
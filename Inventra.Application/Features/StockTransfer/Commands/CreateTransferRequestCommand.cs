using Inventra.Application.Common.CQRS;

namespace Inventra.Application.Features.StockTransfer.Commands
{
    /// <summary>
    /// Command to create a new stock transfer request.
    /// Returns the ID of the created transfer request.
    /// </summary>
    public record CreateTransferRequestCommand(
        Guid ProductId,
        Guid FromWarehouseId,
        Guid ToWarehouseId,
        int Quantity) : ICommand<Guid>;
}
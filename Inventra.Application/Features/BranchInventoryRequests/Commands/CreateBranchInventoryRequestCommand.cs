using Inventra.Application.Common.CQRS;

namespace Inventra.Application.Features.BranchInventoryRequests.Commands
{
    /// <summary>
    /// Command to submit a new stock request from a branch to the warehouse.
    /// Returns the ID of the created request.
    ///
    /// If the requested quantity meets or exceeds the configured LargeRequestThreshold,
    /// a real-time notification is dispatched to the responsible warehouse manager.
    /// </summary>
    public record CreateBranchInventoryRequestCommand(
        Guid BranchId,
        Guid ProductId,
        Guid WarehouseId,
        int RequestedQuantity,
        string? Notes,

        /// <summary>
        /// The user ID of the warehouse manager who should be notified
        /// if this request qualifies as a large request.
        /// </summary>
        string WarehouseManagerUserId) : ICommand<Guid>;
}

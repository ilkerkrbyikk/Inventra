using MediatR;

namespace Inventra.Application.Common.Notifications
{
    /// <summary>
    /// Published when a branch submits an inventory request whose quantity
    /// meets or exceeds the configured LargeRequestThreshold.
    ///
    /// Consumers of this notification are responsible for delivering
    /// real-time alerts to the appropriate warehouse manager(s).
    /// </summary>
    /// <param name="RequestId">The ID of the newly created BranchInventoryRequest.</param>
    /// <param name="BranchId">The branch that submitted the request.</param>
    /// <param name="BranchName">Human-readable branch name for the notification message.</param>
    /// <param name="ProductId">The product being requested.</param>
    /// <param name="ProductName">Human-readable product name for the notification message.</param>
    /// <param name="RequestedQuantity">The quantity the branch is requesting.</param>
    /// <param name="LargeRequestThreshold">The configured threshold that was met or exceeded.</param>
    /// <param name="WarehouseManagerUserId">
    /// The user ID of the warehouse manager who should receive the notification.
    /// </param>
    public record LargeInventoryRequestedNotification(
        Guid RequestId,
        Guid BranchId,
        string BranchName,
        Guid ProductId,
        string ProductName,
        int RequestedQuantity,
        int LargeRequestThreshold,
        string WarehouseManagerUserId) : INotification;
}

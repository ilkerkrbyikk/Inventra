namespace Inventra.Application.Interfaces
{
    /// <summary>
    /// Defines the contract for pushing real-time inventory notifications
    /// to connected warehouse managers.
    ///
    /// Implemented in the Presentation layer.
    /// Called by Application-layer MediatR notification handlers.
    /// This keeps the Application layer free of SignalR and other transport concerns.
    /// </summary>
    public interface IInventoryNotificationService
    {
        /// <summary>
        /// Pushes a low-stock alert to the specified warehouse manager.
        /// </summary>
        /// <param name="userId">The user ID of the warehouse manager to notify.</param>
        /// <param name="productId">The product that is critically low.</param>
        /// <param name="productName">Human-readable product name.</param>
        /// <param name="currentStock">Current stock level at the time of the alert.</param>
        /// <param name="threshold">The configured critical threshold that was breached.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task NotifyLowStockAsync(
            string userId,
            Guid productId,
            string productName,
            int currentStock,
            int threshold,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Pushes a large-inventory-request alert to the specified warehouse manager.
        /// </summary>
        /// <param name="userId">The user ID of the warehouse manager to notify.</param>
        /// <param name="requestId">The ID of the branch inventory request.</param>
        /// <param name="branchName">Human-readable branch name.</param>
        /// <param name="productName">Human-readable product name.</param>
        /// <param name="requestedQuantity">The quantity requested by the branch.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task NotifyLargeInventoryRequestAsync(
            string userId,
            Guid requestId,
            string branchName,
            string productName,
            int requestedQuantity,
            CancellationToken cancellationToken = default);
    }
}

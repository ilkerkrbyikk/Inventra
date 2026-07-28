using MediatR;

namespace Inventra.Application.Common.Notifications
{
    /// <summary>
    /// Published when a product's stock quantity drops at or below its
    /// configured CriticalStockThreshold after any stock-reducing operation.
    ///
    /// Consumers of this notification are responsible for delivering
    /// real-time alerts to the appropriate warehouse manager(s).
    /// </summary>
    /// <param name="ProductId">The product whose stock has reached a critical level.</param>
    /// <param name="ProductName">Human-readable product name for the notification message.</param>
    /// <param name="CurrentStockQuantity">Stock quantity at the moment the threshold was crossed.</param>
    /// <param name="CriticalStockThreshold">The configured threshold that was breached.</param>
    /// <param name="WarehouseManagerUserId">
    /// The user ID of the warehouse manager who should receive the notification.
    /// </param>
    public record LowStockDetectedNotification(
        Guid ProductId,
        string ProductName,
        int CurrentStockQuantity,
        int CriticalStockThreshold,
        string WarehouseManagerUserId) : INotification;
}

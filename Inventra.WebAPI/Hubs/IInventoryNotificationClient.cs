namespace Inventra.WebAPI.Hubs
{
    /// <summary>
    /// Client methods invoked by the inventory notification hub.
    /// </summary>
    public interface IInventoryNotificationClient
    {
        Task ReceiveLowStockNotification(LowStockNotificationPayload notification);

        Task ReceiveLargeInventoryRequestNotification(LargeInventoryRequestNotificationPayload notification);
    }

    /// <summary>
    /// Public low-stock payload. Recipient identity is intentionally excluded.
    /// </summary>
    public sealed record LowStockNotificationPayload(
        Guid ProductId,
        string ProductName,
        int CurrentStock,
        int CriticalStockThreshold);

    /// <summary>
    /// Public large-request payload. Recipient identity is intentionally excluded.
    /// </summary>
    public sealed record LargeInventoryRequestNotificationPayload(
        Guid RequestId,
        string BranchName,
        string ProductName,
        int RequestedQuantity);
}

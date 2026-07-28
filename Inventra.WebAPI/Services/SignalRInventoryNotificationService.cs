using Inventra.Application.Interfaces;
using Inventra.WebAPI.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Inventra.WebAPI.Services
{
    /// <summary>
    /// SignalR implementation of the application notification transport.
    ///
    /// Until authenticated warehouse-manager identities are available, alerts are broadcast
    /// to connected clients. The transport contract preserves the recipient ID so this can
    /// later be changed to authenticated user or role-group delivery without touching
    /// application event handlers.
    /// </summary>
    public sealed class SignalRInventoryNotificationService : IInventoryNotificationService
    {
        private readonly IHubContext<InventoryNotificationHub, IInventoryNotificationClient> _hubContext;

        public SignalRInventoryNotificationService(
            IHubContext<InventoryNotificationHub, IInventoryNotificationClient> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task NotifyLowStockAsync(
            string userId,
            Guid productId,
            string productName,
            int currentStock,
            int threshold,
            CancellationToken cancellationToken = default)
        {
            var payload = new LowStockNotificationPayload(
                productId,
                productName,
                currentStock,
                threshold);

            return _hubContext.Clients.All.ReceiveLowStockNotification(payload);
        }

        public Task NotifyLargeInventoryRequestAsync(
            string userId,
            Guid requestId,
            string branchName,
            string productName,
            int requestedQuantity,
            CancellationToken cancellationToken = default)
        {
            var payload = new LargeInventoryRequestNotificationPayload(
                requestId,
                branchName,
                productName,
                requestedQuantity);

            return _hubContext.Clients.All.ReceiveLargeInventoryRequestNotification(payload);
        }
    }
}

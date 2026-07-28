using Inventra.Application.Interfaces;
using MediatR;

namespace Inventra.Application.Common.Notifications
{
    /// <summary>
    /// Delivers low-stock application events through the configured notification transport.
    /// </summary>
    public sealed class LowStockDetectedNotificationHandler
        : INotificationHandler<LowStockDetectedNotification>
    {
        private readonly IInventoryNotificationService _notificationService;

        public LowStockDetectedNotificationHandler(
            IInventoryNotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public Task Handle(
            LowStockDetectedNotification notification,
            CancellationToken cancellationToken)
        {
            return _notificationService.NotifyLowStockAsync(
                notification.WarehouseManagerUserId,
                notification.ProductId,
                notification.ProductName,
                notification.CurrentStockQuantity,
                notification.CriticalStockThreshold,
                cancellationToken);
        }
    }
}

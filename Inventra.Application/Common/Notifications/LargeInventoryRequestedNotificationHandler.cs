using Inventra.Application.Interfaces;
using MediatR;

namespace Inventra.Application.Common.Notifications
{
    /// <summary>
    /// Delivers large inventory-request application events through the configured notification transport.
    /// </summary>
    public sealed class LargeInventoryRequestedNotificationHandler
        : INotificationHandler<LargeInventoryRequestedNotification>
    {
        private readonly IInventoryNotificationService _notificationService;

        public LargeInventoryRequestedNotificationHandler(
            IInventoryNotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public Task Handle(
            LargeInventoryRequestedNotification notification,
            CancellationToken cancellationToken)
        {
            return _notificationService.NotifyLargeInventoryRequestAsync(
                notification.WarehouseManagerUserId,
                notification.RequestId,
                notification.BranchName,
                notification.ProductName,
                notification.RequestedQuantity,
                cancellationToken);
        }
    }
}

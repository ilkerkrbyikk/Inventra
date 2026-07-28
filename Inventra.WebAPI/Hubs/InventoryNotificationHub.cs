using Microsoft.AspNetCore.SignalR;

namespace Inventra.WebAPI.Hubs
{
    /// <summary>
    /// SignalR endpoint for server-pushed inventory notifications.
    ///
    /// This hub intentionally exposes no client-invocable operations. The server pushes
    /// events through <see cref="IHubContext{THub, TClient}"/> after application events occur.
    /// </summary>
    public sealed class InventoryNotificationHub : Hub<IInventoryNotificationClient>
    {
    }
}

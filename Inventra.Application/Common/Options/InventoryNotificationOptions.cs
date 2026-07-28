namespace Inventra.Application.Common.Options
{
    /// <summary>
    /// Strongly-typed configuration options for inventory real-time notifications.
    /// Bound from the "InventoryNotifications" section in appsettings.json.
    /// </summary>
    public class InventoryNotificationOptions
    {
        /// <summary>
        /// The configuration section key used for binding.
        /// </summary>
        public const string SectionName = "InventoryNotifications";

        /// <summary>
        /// The minimum requested quantity that qualifies a branch inventory request
        /// as "large" and triggers an immediate notification to the warehouse manager.
        /// Defaults to 100 units.
        /// </summary>
        public int LargeRequestThreshold { get; set; } = 100;
    }
}

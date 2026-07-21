namespace Inventra.Infrastructure.Audit
{
    /// <summary>
    /// Ambient context for storing audit-related information during request processing.
    /// Uses AsyncLocal to maintain isolation between concurrent requests.
    /// </summary>
    public class AuditContext
    {
        private static readonly AsyncLocal<AuditContextData> _context = new();

        /// <summary>
        /// Gets or sets the current audit context data for the async flow.
        /// </summary>
        public static AuditContextData Current
        {
            get => _context.Value ??= new AuditContextData();
            set => _context.Value = value;
        }

        /// <summary>
        /// Sets the current user information in the audit context.
        /// </summary>
        /// <param name="userId">ID of the current user.</param>
        /// <param name="username">Username of the current user.</param>
        public static void SetCurrentUser(string? userId, string? username)
        {
            Current.UserId = userId;
            Current.Username = username;
        }

        /// <summary>
        /// Sets the IP address in the audit context.
        /// </summary>
        /// <param name="ipAddress">IP address of the request.</param>
        public static void SetIpAddress(string? ipAddress)
        {
            Current.IpAddress = ipAddress;
        }

        /// <summary>
        /// Clears the audit context data.
        /// </summary>
        public static void Clear()
        {
            _context.Value = null;
        }
    }

    /// <summary>
    /// Data carrier for audit context information.
    /// </summary>
    public class AuditContextData
    {
        /// <summary>
        /// ID of the current user performing the action.
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Username of the current user performing the action.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// IP address from which the request originated.
        /// </summary>
        public string? IpAddress { get; set; }
    }
}
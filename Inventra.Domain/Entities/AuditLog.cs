using Inventra.Domain.Enums;
using System.Text.Json;

namespace Inventra.Domain.Entities
{
    /// <summary>
    /// Represents an audit log entry for tracking changes to entities.
    /// Captures entity metadata, action type, timestamps, user information, and property changes.
    /// </summary>
    public class AuditLog : BaseEntity
    {
        /// <summary>
        /// Full type name of the entity that was audited (e.g., "Inventra.Domain.Entities.Product").
        /// </summary>
        public string EntityType { get; set; } = string.Empty;

        /// <summary>
        /// Primary key value of the audited entity as a string.
        /// </summary>
        public string EntityId { get; set; } = string.Empty;

        /// <summary>
        /// Type of action performed on the entity (Create, Update, Delete).
        /// </summary>
        public AuditActionType Action { get; set; }

        /// <summary>
        /// Timestamp of the audit action in UTC.
        /// </summary>
        public DateTime ActionTimestamp { get; set; }

        /// <summary>
        /// ID of the user who performed the action. Null if user information is not available.
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Username of the user who performed the action. Null if user information is not available.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// IP address from which the action was performed. Null if IP information is not available.
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// JSON-serialized array of property changes. Only modified properties are included.
        /// For Create operations, contains all properties with null old values.
        /// For Delete operations, contains all properties with null new values.
        /// </summary>
        public string? ChangedProperties { get; set; }

        /// <summary>
        /// Initializes a new instance of the AuditLog class.
        /// </summary>
        public AuditLog()
        {
        }

        /// <summary>
        /// Initializes a new instance of the AuditLog class with specified parameters.
        /// </summary>
        public AuditLog(
            string entityType,
            string entityId,
            AuditActionType action,
            DateTime actionTimestamp,
            string? userId = null,
            string? username = null,
            string? ipAddress = null,
            string? changedProperties = null)
        {
            EntityType = entityType ?? throw new ArgumentNullException(nameof(entityType));
            EntityId = entityId ?? throw new ArgumentNullException(nameof(entityId));
            Action = action;
            ActionTimestamp = actionTimestamp;
            UserId = userId;
            Username = username;
            IpAddress = ipAddress;
            ChangedProperties = changedProperties;
        }

        /// <summary>
        /// Gets the user display name. Returns "Unknown User" if user information is not available.
        /// </summary>
        public string GetUserDisplay()
            => !string.IsNullOrEmpty(Username) ? Username : UserId ?? "Unknown User";

        /// <summary>
        /// Checks if this audit log represents a sensitive operation.
        /// Sensitive operations have limited property change information.
        /// </summary>
        public bool HasSensitiveChanges()
            => ChangedProperties?.Contains("***") ?? false;
    }
}
using Inventra.Domain.Enums;

namespace Inventra.Application.DTOs
{
    /// <summary>
    /// Data transfer object for audit log entries.
    /// Used for exposing audit log information through API endpoints.
    /// </summary>
    public class AuditLogDto
    {
        /// <summary>
        /// Unique identifier of the audit log entry.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Full type name of the audited entity.
        /// </summary>
        public string EntityType { get; set; } = string.Empty;

        /// <summary>
        /// Primary key of the audited entity as a string.
        /// </summary>
        public string EntityId { get; set; } = string.Empty;

        /// <summary>
        /// Type of action performed (Create, Update, Delete).
        /// </summary>
        public AuditActionType Action { get; set; }

        /// <summary>
        /// Timestamp of the action in UTC.
        /// </summary>
        public DateTime ActionTimestamp { get; set; }

        /// <summary>
        /// Display name of the user who performed the action.
        /// </summary>
        public string UserDisplay { get; set; } = string.Empty;

        /// <summary>
        /// IP address from which the action was performed.
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// JSON-serialized array of property changes.
        /// </summary>
        public string? ChangedProperties { get; set; }

        /// <summary>
        /// Indicates whether this audit log contains sensitive data.
        /// </summary>
        public bool HasSensitiveChanges { get; set; }
    }
}
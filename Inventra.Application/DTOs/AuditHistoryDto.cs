namespace Inventra.Application.DTOs
{
    /// <summary>
    /// Data transfer object for audit history queries.
    /// Used for retrieving audit history for a specific entity.
    /// </summary>
    public class AuditHistoryDto
    {
        /// <summary>
        /// Full type name of the entity.
        /// </summary>
        public string EntityType { get; set; } = string.Empty;

        /// <summary>
        /// Primary key of the entity as a string.
        /// </summary>
        public string EntityId { get; set; } = string.Empty;

        /// <summary>
        /// Collection of audit log entries for the entity, ordered by timestamp descending.
        /// </summary>
        public ICollection<AuditLogDto> AuditLogs { get; set; } = new List<AuditLogDto>();

        /// <summary>
        /// Total number of audit entries for the entity.
        /// </summary>
        public int TotalCount => AuditLogs.Count;
    }
}
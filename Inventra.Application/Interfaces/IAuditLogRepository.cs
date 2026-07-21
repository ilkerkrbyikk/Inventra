using Inventra.Domain.Entities;
using Inventra.Domain.Enums;

namespace Inventra.Application.Interfaces
{
    /// <summary>
    /// Repository contract for audit log data access.
    /// Extends the generic repository with audit-specific query operations.
    /// </summary>
    public interface IAuditLogRepository : IGenericRepository<AuditLog>
    {
        /// <summary>
        /// Retrieves audit logs for a specific entity type and ID.
        /// </summary>
        /// <param name="entityType">Full type name of the entity.</param>
        /// <param name="entityId">Primary key of the entity.</param>
        /// <returns>Collection of audit logs for the specified entity.</returns>
        Task<IEnumerable<AuditLog>> GetByEntityAsync(string entityType, string entityId);

        /// <summary>
        /// Retrieves audit logs for a specific action type.
        /// </summary>
        /// <param name="action">Action type to filter by.</param>
        /// <param name="limit">Maximum number of results to return.</param>
        /// <returns>Collection of audit logs for the specified action type.</returns>
        Task<IEnumerable<AuditLog>> GetByActionAsync(AuditActionType action, int limit = 100);

        /// <summary>
        /// Retrieves audit logs for a specific user.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <param name="limit">Maximum number of results to return.</param>
        /// <returns>Collection of audit logs for the specified user.</returns>
        Task<IEnumerable<AuditLog>> GetByUserIdAsync(string userId, int limit = 100);

        /// <summary>
        /// Retrieves audit logs within a specific date range.
        /// </summary>
        /// <param name="startDate">Start date (UTC).</param>
        /// <param name="endDate">End date (UTC).</param>
        /// <param name="limit">Maximum number of results to return.</param>
        /// <returns>Collection of audit logs within the date range.</returns>
        Task<IEnumerable<AuditLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, int limit = 1000);

        /// <summary>
        /// Retrieves the most recent audit logs.
        /// </summary>
        /// <param name="limit">Number of recent logs to retrieve.</param>
        /// <returns>Collection of the most recent audit logs.</returns>
        Task<IEnumerable<AuditLog>> GetRecentAsync(int limit = 100);
    }
}
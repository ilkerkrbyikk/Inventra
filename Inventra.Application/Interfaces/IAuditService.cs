using Inventra.Domain.Enums;
using Inventra.Domain.ValueObjects;

namespace Inventra.Application.Interfaces
{
    /// <summary>
    /// Service contract for creating and managing audit log entries.
    /// Handles the business logic of audit operations without database concerns.
    /// </summary>
    public interface IAuditService
    {
        /// <summary>
        /// Creates an audit log entry for an entity change.
        /// </summary>
        /// <param name="entityType">Full type name of the audited entity.</param>
        /// <param name="entityId">Primary key of the audited entity as string.</param>
        /// <param name="action">Type of action performed (Create, Update, Delete).</param>
        /// <param name="propertyChanges">Collection of property changes. Can be empty for operations without tracked changes.</param>
        /// <param name="userId">ID of the user performing the action. Optional.</param>
        /// <param name="username">Username of the user performing the action. Optional.</param>
        /// <param name="ipAddress">IP address from which the action was performed. Optional.</param>
        /// <returns>The created AuditLog entity.</returns>
        Task<Domain.Entities.AuditLog> CreateAuditLogAsync(
            string entityType,
            string entityId,
            AuditActionType action,
            IEnumerable<PropertyChange> propertyChanges,
            string? userId = null,
            string? username = null,
            string? ipAddress = null);

        /// <summary>
        /// Retrieves all audit log entries for a specific entity.
        /// </summary>
        /// <param name="entityType">Full type name of the entity to filter by.</param>
        /// <param name="entityId">Primary key of the entity to filter by.</param>
        /// <returns>Collection of audit log entries for the specified entity.</returns>
        Task<IEnumerable<Domain.Entities.AuditLog>> GetEntityAuditHistoryAsync(string entityType, string entityId);

        /// <summary>
        /// Retrieves audit log entries for a specific action type.
        /// </summary>
        /// <param name="action">Action type to filter by (Create, Update, Delete).</param>
        /// <param name="limit">Maximum number of results to return. Default is 100.</param>
        /// <returns>Collection of recent audit log entries for the specified action type.</returns>
        Task<IEnumerable<Domain.Entities.AuditLog>> GetAuditLogsByActionAsync(
            AuditActionType action,
            int limit = 100);

        /// <summary>
        /// Retrieves audit log entries for a specific user.
        /// </summary>
        /// <param name="userId">ID of the user to filter by.</param>
        /// <param name="limit">Maximum number of results to return. Default is 100.</param>
        /// <returns>Collection of recent audit log entries for the specified user.</returns>
        Task<IEnumerable<Domain.Entities.AuditLog>> GetUserAuditHistoryAsync(
            string userId,
            int limit = 100);

        /// <summary>
        /// Retrieves audit log entries within a specific date range.
        /// </summary>
        /// <param name="startDate">Start date for the range (UTC).</param>
        /// <param name="endDate">End date for the range (UTC).</param>
        /// <param name="limit">Maximum number of results to return. Default is 1000.</param>
        /// <returns>Collection of audit log entries within the specified date range.</returns>
        Task<IEnumerable<Domain.Entities.AuditLog>> GetAuditLogsByDateRangeAsync(
            DateTime startDate,
            DateTime endDate,
            int limit = 1000);
    }
}
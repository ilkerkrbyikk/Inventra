using Inventra.Application.Interfaces;
using Inventra.Domain.Entities;
using Inventra.Domain.Enums;
using Inventra.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Inventra.Infrastructure.Audit
{
    /// <summary>
    /// Service implementation for audit logging operations.
    /// Provides business logic for creating and querying audit logs.
    /// Handles graceful failures without affecting business transactions.
    /// </summary>
    public class AuditService : IAuditService
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly ILogger<AuditService> _logger;

        /// <summary>
        /// Initializes a new instance of the AuditService class.
        /// </summary>
        /// <param name="auditLogRepository">Repository for audit log persistence.</param>
        /// <param name="logger">Logger for recording audit service operations.</param>
        public AuditService(
            IAuditLogRepository auditLogRepository,
            ILogger<AuditService> logger)
        {
            _auditLogRepository = auditLogRepository ?? throw new ArgumentNullException(nameof(auditLogRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Creates an audit log entry for an entity change.
        /// Fails gracefully without throwing exceptions that would interrupt business operations.
        /// </summary>
        public async Task<AuditLog> CreateAuditLogAsync(
            string entityType,
            string entityId,
            AuditActionType action,
            IEnumerable<PropertyChange> propertyChanges,
            string? userId = null,
            string? username = null,
            string? ipAddress = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(entityType))
                    throw new ArgumentException("Entity type cannot be null or empty.", nameof(entityType));

                if (string.IsNullOrWhiteSpace(entityId))
                    throw new ArgumentException("Entity ID cannot be null or empty.", nameof(entityId));

                var changesList = propertyChanges?.ToList() ?? [];

                var auditLog = new AuditLog(
                    entityType: entityType,
                    entityId: entityId,
                    action: action,
                    actionTimestamp: DateTime.UtcNow,
                    userId: userId,
                    username: username,
                    ipAddress: ipAddress,
                    changedProperties: SerializeChanges(changesList));

                await _auditLogRepository.AddAsync(auditLog);

                _logger.LogInformation(
                    "Audit log created successfully. EntityType={EntityType}, EntityId={EntityId}, Action={Action}",
                    entityType, entityId, action);

                return auditLog;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error creating audit log. EntityType={EntityType}, EntityId={EntityId}, Action={Action}",
                    entityType, entityId, action);

                // Return empty audit log to avoid breaking business operations
                return new AuditLog
                {
                    EntityType = entityType ?? "Unknown",
                    EntityId = entityId ?? "Unknown",
                    Action = action,
                    ActionTimestamp = DateTime.UtcNow
                };
            }
        }

        /// <summary>
        /// Retrieves all audit log entries for a specific entity.
        /// </summary>
        public async Task<IEnumerable<AuditLog>> GetEntityAuditHistoryAsync(string entityType, string entityId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(entityType) || string.IsNullOrWhiteSpace(entityId))
                    return [];

                var logs = await _auditLogRepository.GetByEntityAsync(entityType, entityId);
                return logs;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error retrieving audit history. EntityType={EntityType}, EntityId={EntityId}",
                    entityType, entityId);

                return [];
            }
        }

        /// <summary>
        /// Retrieves audit log entries for a specific action type.
        /// </summary>
        public async Task<IEnumerable<AuditLog>> GetAuditLogsByActionAsync(
            AuditActionType action,
            int limit = 100)
        {
            try
            {
                var validLimit = Math.Min(Math.Max(limit, 1), 10000);
                var logs = await _auditLogRepository.GetByActionAsync(action, validLimit);
                return logs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving audit logs by action. Action={Action}", action);
                return [];
            }
        }

        /// <summary>
        /// Retrieves audit log entries for a specific user.
        /// </summary>
        public async Task<IEnumerable<AuditLog>> GetUserAuditHistoryAsync(
            string userId,
            int limit = 100)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                    return [];

                var validLimit = Math.Min(Math.Max(limit, 1), 10000);
                var logs = await _auditLogRepository.GetByUserIdAsync(userId, validLimit);
                return logs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving audit history for user. UserId={UserId}", userId);
                return [];
            }
        }

        /// <summary>
        /// Retrieves audit log entries within a specific date range.
        /// </summary>
        public async Task<IEnumerable<AuditLog>> GetAuditLogsByDateRangeAsync(
            DateTime startDate,
            DateTime endDate,
            int limit = 1000)
        {
            try
            {
                if (startDate > endDate)
                    return [];

                var validLimit = Math.Min(Math.Max(limit, 1), 10000);
                var logs = await _auditLogRepository.GetByDateRangeAsync(startDate, endDate, validLimit);
                return logs;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error retrieving audit logs by date range. StartDate={StartDate}, EndDate={EndDate}",
                    startDate, endDate);

                return [];
            }
        }

        /// <summary>
        /// Serializes property changes to JSON format.
        /// </summary>
        private static string? SerializeChanges(List<PropertyChange> changes)
        {
            if (changes.Count == 0)
                return null;

            try
            {
                var changeData = changes.Select(c => new
                {
                    c.PropertyName,
                    c.OldValue,
                    c.NewValue
                }).ToList();

                return System.Text.Json.JsonSerializer.Serialize(changeData);
            }
            catch
            {
                return null;
            }
        }
    }
}
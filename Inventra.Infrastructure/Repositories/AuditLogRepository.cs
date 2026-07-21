using Inventra.Application.Interfaces;
using Inventra.Domain.Entities;
using Inventra.Domain.Enums;
using Inventra.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventra.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for audit log data access.
    /// Provides specialized query methods for audit log retrieval.
    /// </summary>
    public class AuditLogRepository : GenericRepository<AuditLog>, IAuditLogRepository
    {
        /// <summary>
        /// Initializes a new instance of the AuditLogRepository class.
        /// </summary>
        /// <param name="context">Database context for audit log operations.</param>
        public AuditLogRepository(DatabaseContext context) : base(context)
        {
        }

        /// <summary>
        /// Retrieves audit logs for a specific entity type and ID.
        /// Results are ordered by action timestamp in descending order.
        /// </summary>
        public async Task<IEnumerable<AuditLog>> GetByEntityAsync(string entityType, string entityId)
        {
            try
            {
                var logs = await _context.AuditLogs
                    .Where(a => a.EntityType == entityType && a.EntityId == entityId)
                    .OrderByDescending(a => a.ActionTimestamp)
                    .ToListAsync();

                return logs;
            }
            catch
            {
                return [];
            }
        }

        /// <summary>
        /// Retrieves audit logs for a specific action type.
        /// Results are ordered by action timestamp in descending order.
        /// </summary>
        public async Task<IEnumerable<AuditLog>> GetByActionAsync(AuditActionType action, int limit = 100)
        {
            try
            {
                var validLimit = Math.Min(Math.Max(limit, 1), 10000);

                var logs = await _context.AuditLogs
                    .Where(a => a.Action == action)
                    .OrderByDescending(a => a.ActionTimestamp)
                    .Take(validLimit)
                    .ToListAsync();

                return logs;
            }
            catch
            {
                return [];
            }
        }

        /// <summary>
        /// Retrieves audit logs for a specific user.
        /// Results are ordered by action timestamp in descending order.
        /// </summary>
        public async Task<IEnumerable<AuditLog>> GetByUserIdAsync(string userId, int limit = 100)
        {
            try
            {
                var validLimit = Math.Min(Math.Max(limit, 1), 10000);

                var logs = await _context.AuditLogs
                    .Where(a => a.UserId == userId)
                    .OrderByDescending(a => a.ActionTimestamp)
                    .Take(validLimit)
                    .ToListAsync();

                return logs;
            }
            catch
            {
                return [];
            }
        }

        /// <summary>
        /// Retrieves audit logs within a specific date range.
        /// Results are ordered by action timestamp in descending order.
        /// </summary>
        public async Task<IEnumerable<AuditLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, int limit = 1000)
        {
            try
            {
                var validLimit = Math.Min(Math.Max(limit, 1), 10000);

                var logs = await _context.AuditLogs
                    .Where(a => a.ActionTimestamp >= startDate && a.ActionTimestamp <= endDate)
                    .OrderByDescending(a => a.ActionTimestamp)
                    .Take(validLimit)
                    .ToListAsync();

                return logs;
            }
            catch
            {
                return [];
            }
        }

        /// <summary>
        /// Retrieves the most recent audit logs.
        /// </summary>
        public async Task<IEnumerable<AuditLog>> GetRecentAsync(int limit = 100)
        {
            try
            {
                var validLimit = Math.Min(Math.Max(limit, 1), 10000);

                var logs = await _context.AuditLogs
                    .OrderByDescending(a => a.ActionTimestamp)
                    .Take(validLimit)
                    .ToListAsync();

                return logs;
            }
            catch
            {
                return [];
            }
        }
    }
}
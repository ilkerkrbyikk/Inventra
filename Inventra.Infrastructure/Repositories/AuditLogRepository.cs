using Inventra.Application.Interfaces;
using Inventra.Domain.Entities;
using Inventra.Domain.Enums;
using Inventra.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventra.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for audit log data access.
    /// Provides specialized queries for audit log retrieval.
    /// </summary>
    public class AuditLogRepository : GenericRepository<AuditLog>, IAuditLogRepository
    {
        public AuditLogRepository(DatabaseContext context) : base(context)
        {
        }

        /// <inheritdoc />
        public async Task<IEnumerable<AuditLog>> GetByEntityAsync(string entityType, string entityId)
        {
            return await DbSet
                .Where(al => al.EntityType == entityType && al.EntityId == entityId)
                .OrderByDescending(al => al.ActionTimestamp)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<AuditLog>> GetByActionAsync(AuditActionType action, int limit = 100)
        {
            return await DbSet
                .Where(al => al.Action == action)
                .OrderByDescending(al => al.ActionTimestamp)
                .Take(limit)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<AuditLog>> GetByUserIdAsync(string userId, int limit = 100)
        {
            return await DbSet
                .Where(al => al.UserId == userId)
                .OrderByDescending(al => al.ActionTimestamp)
                .Take(limit)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<AuditLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, int limit = 1000)
        {
            return await DbSet
                .Where(al => al.ActionTimestamp >= startDate && al.ActionTimestamp <= endDate)
                .OrderByDescending(al => al.ActionTimestamp)
                .Take(limit)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<AuditLog>> GetRecentAsync(int limit = 100)
        {
            return await DbSet
                .OrderByDescending(al => al.ActionTimestamp)
                .Take(limit)
                .ToListAsync();
        }
    }
}
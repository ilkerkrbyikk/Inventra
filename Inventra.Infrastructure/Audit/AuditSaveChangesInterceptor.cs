using Inventra.Domain.Entities;
using Inventra.Domain.Enums;
using Inventra.Domain.Interfaces;
using Inventra.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;

namespace Inventra.Infrastructure.Audit
{
    /// <summary>
    /// Entity Framework Core interceptor that automatically captures audit logs for all data changes.
    /// Hooks into the SaveChanges pipeline to detect Add, Update, and Delete operations.
    /// </summary>
    public class AuditSaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly AuditChangeTracker _changeTracker;
        private List<AuditLog>? _auditLogs;

        /// <summary>
        /// Initializes a new instance of the AuditSaveChangesInterceptor class.
        /// </summary>
        /// <param name="changeTracker">Change tracker for detecting modifications.</param>
        public AuditSaveChangesInterceptor(AuditChangeTracker changeTracker)
        {
            _changeTracker = changeTracker ?? throw new ArgumentNullException(nameof(changeTracker));
        }

        /// <summary>
        /// Executes before SaveChanges is called on the DbContext.
        /// Detects and creates audit log entries for all pending changes.
        /// </summary>
        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            if (eventData.Context == null)
                return result;

            _auditLogs = CaptureAuditLogs(eventData.Context);
            return result;
        }

        /// <summary>
        /// Executes after SaveChanges completes successfully.
        /// Adds the captured audit logs to the database.
        /// </summary>
        public override int SavedChanges(
            SaveChangesCompletedEventData eventData,
            int result)
        {
            if (_auditLogs?.Count > 0 && eventData.Context != null)
            {
                eventData.Context.Set<AuditLog>().AddRange(_auditLogs);
                eventData.Context.SaveChanges();
            }

            _auditLogs = null;
            return result;
        }

        /// <summary>
        /// Executes before SaveChangesAsync is called on the DbContext.
        /// Detects and creates audit log entries for all pending changes.
        /// </summary>
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            if (eventData.Context == null)
                return new ValueTask<InterceptionResult<int>>(result);

            _auditLogs = CaptureAuditLogs(eventData.Context);
            return new ValueTask<InterceptionResult<int>>(result);
        }

        /// <summary>
        /// Executes after SaveChangesAsync completes successfully.
        /// Adds the captured audit logs to the database.
        /// </summary>
        public override async ValueTask<int> SavedChangesAsync(
            SaveChangesCompletedEventData eventData,
            int result,
            CancellationToken cancellationToken = default)
        {
            if (_auditLogs?.Count > 0 && eventData.Context != null)
            {
                await eventData.Context.Set<AuditLog>().AddRangeAsync(_auditLogs, cancellationToken);
                await eventData.Context.SaveChangesAsync(cancellationToken);
            }

            _auditLogs = null;
            return result;
        }

        /// <summary>
        /// Captures audit logs for all pending changes in the DbContext.
        /// </summary>
        private List<AuditLog> CaptureAuditLogs(DbContext context)
        {
            var auditLogs = new List<AuditLog>();
            var entries = context.ChangeTracker.Entries()
                .Where(e => e.Entity is IAuditable)
                .ToList();

            var auditContext = AuditContext.Current;

            foreach (var entry in entries)
            {
                if (entry.Entity is not BaseEntity)
                    continue;

                AuditLog? auditLog = null;

                switch (entry.State)
                {
                    case EntityState.Added:
                        auditLog = CreateAddAuditLog(entry, auditContext);
                        break;

                    case EntityState.Modified:
                        // Check if only soft delete flag changed (deletion)
                        var isDeletedModified = entry.Property(nameof(BaseEntity.IsDeleted)).IsModified;
                        var currentIsDeleted = (bool)entry.CurrentValues[nameof(BaseEntity.IsDeleted)];
                        var originalIsDeleted = (bool)entry.OriginalValues[nameof(BaseEntity.IsDeleted)];

                        if (isDeletedModified && currentIsDeleted && !originalIsDeleted)
                        {
                            // Soft delete operation
                            auditLog = CreateDeleteAuditLog(entry, auditContext);
                        }
                        else if (isDeletedModified && !currentIsDeleted && originalIsDeleted)
                        {
                            // Restore operation (treat as update)
                            auditLog = CreateUpdateAuditLog(entry, auditContext);
                        }
                        else
                        {
                            // Regular update
                            auditLog = CreateUpdateAuditLog(entry, auditContext);
                        }
                        break;

                    case EntityState.Deleted:
                        auditLog = CreateDeleteAuditLog(entry, auditContext);
                        break;
                }

                if (auditLog != null)
                    auditLogs.Add(auditLog);
            }

            return auditLogs;
        }

        /// <summary>
        /// Creates an audit log for an added entity.
        /// </summary>
        private AuditLog CreateAddAuditLog(
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry,
            AuditContextData auditContext)
        {
            var changes = _changeTracker.GetAddedChanges(entry);
            var changedPropertiesJson = SerializeChanges(changes);

            return new AuditLog(
                entityType: entry.Entity.GetType().FullName!,
                entityId: AuditChangeTracker.GetEntityId(entry),
                action: AuditActionType.Create,
                actionTimestamp: DateTime.UtcNow,
                userId: auditContext.UserId,
                username: auditContext.Username,
                ipAddress: auditContext.IpAddress,
                changedProperties: changedPropertiesJson);
        }

        /// <summary>
        /// Creates an audit log for a modified entity.
        /// </summary>
        private AuditLog CreateUpdateAuditLog(
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry,
            AuditContextData auditContext)
        {
            var changes = _changeTracker.GetModifiedChanges(entry);
            var changedPropertiesJson = SerializeChanges(changes);

            return new AuditLog(
                entityType: entry.Entity.GetType().FullName!,
                entityId: AuditChangeTracker.GetEntityId(entry),
                action: AuditActionType.Update,
                actionTimestamp: DateTime.UtcNow,
                userId: auditContext.UserId,
                username: auditContext.Username,
                ipAddress: auditContext.IpAddress,
                changedProperties: changedPropertiesJson);
        }

        /// <summary>
        /// Creates an audit log for a deleted entity.
        /// </summary>
        private AuditLog CreateDeleteAuditLog(
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry,
            AuditContextData auditContext)
        {
            var changes = _changeTracker.GetDeletedChanges(entry);
            var changedPropertiesJson = SerializeChanges(changes);

            return new AuditLog(
                entityType: entry.Entity.GetType().FullName!,
                entityId: AuditChangeTracker.GetEntityId(entry),
                action: AuditActionType.Delete,
                actionTimestamp: DateTime.UtcNow,
                userId: auditContext.UserId,
                username: auditContext.Username,
                ipAddress: auditContext.IpAddress,
                changedProperties: changedPropertiesJson);
        }

        /// <summary>
        /// Serializes a collection of property changes to JSON.
        /// Returns null if the collection is empty.
        /// </summary>
        private static string? SerializeChanges(IEnumerable<PropertyChange> changes)
        {
            var changeList = changes.ToList();
            if (changeList.Count == 0)
                return null;

            var changeData = changeList.Select(c => new
            {
                c.PropertyName,
                c.OldValue,
                c.NewValue
            }).ToList();

            return JsonSerializer.Serialize(changeData);
        }
    }
}
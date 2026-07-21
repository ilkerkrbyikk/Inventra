using Inventra.Domain.Entities;
using Inventra.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Inventra.Infrastructure.Audit
{
    /// <summary>
    /// Tracks and extracts changes from Entity Framework Core's change tracker.
    /// Identifies modified properties and captures their old and new values.
    /// </summary>
    public class AuditChangeTracker
    {
        private readonly SensitivePropertyFilter _sensitivePropertyFilter;

        /// <summary>
        /// Initializes a new instance of the AuditChangeTracker class.
        /// </summary>
        /// <param name="sensitivePropertyFilter">Filter for masking sensitive properties.</param>
        public AuditChangeTracker(SensitivePropertyFilter sensitivePropertyFilter)
        {
            _sensitivePropertyFilter = sensitivePropertyFilter ?? throw new ArgumentNullException(nameof(sensitivePropertyFilter));
        }

        /// <summary>
        /// Gets property changes for an added entity.
        /// </summary>
        /// <param name="entry">Entity Framework change entry for the added entity.</param>
        /// <returns>Collection of property changes with null old values.</returns>
        public IEnumerable<PropertyChange> GetAddedChanges(EntityEntry entry)
        {
            var changes = new List<PropertyChange>();
            var properties = entry.CurrentValues.Properties;

            foreach (var property in properties)
            {
                // Skip navigation properties, shadow properties, and key/timestamp properties
                if (ShouldSkipProperty(property.Name, entry))
                    continue;

                var newValue = entry.CurrentValues[property]?.ToString();
                var filteredValue = _sensitivePropertyFilter.FilterValue(property.Name, newValue);

                changes.Add(new PropertyChange(property.Name, null, filteredValue));
            }

            return changes;
        }

        /// <summary>
        /// Gets property changes for a modified entity.
        /// Only includes properties that actually changed.
        /// </summary>
        /// <param name="entry">Entity Framework change entry for the modified entity.</param>
        /// <returns>Collection of property changes for modified properties only.</returns>
        public IEnumerable<PropertyChange> GetModifiedChanges(EntityEntry entry)
        {
            var changes = new List<PropertyChange>();
            var properties = entry.CurrentValues.Properties;

            foreach (var property in properties)
            {
                // Skip unchanged properties, navigation properties, and key/timestamp properties
                if (!entry.Property(property.Name).IsModified || ShouldSkipProperty(property.Name, entry))
                    continue;

                var oldValue = entry.OriginalValues[property]?.ToString();
                var newValue = entry.CurrentValues[property]?.ToString();

                // Skip if values are equal (shouldn't happen, but defensive check)
                if (oldValue == newValue)
                    continue;

                var filteredOldValue = _sensitivePropertyFilter.FilterValue(property.Name, oldValue);
                var filteredNewValue = _sensitivePropertyFilter.FilterValue(property.Name, newValue);

                changes.Add(new PropertyChange(property.Name, filteredOldValue, filteredNewValue));
            }

            return changes;
        }

        /// <summary>
        /// Gets property changes for a deleted entity.
        /// </summary>
        /// <param name="entry">Entity Framework change entry for the deleted entity.</param>
        /// <returns>Collection of property changes with null new values.</returns>
        public IEnumerable<PropertyChange> GetDeletedChanges(EntityEntry entry)
        {
            var changes = new List<PropertyChange>();
            var properties = entry.OriginalValues.Properties;

            foreach (var property in properties)
            {
                // Skip navigation properties, shadow properties, and key/timestamp properties
                if (ShouldSkipProperty(property.Name, entry))
                    continue;

                var oldValue = entry.OriginalValues[property]?.ToString();
                var filteredValue = _sensitivePropertyFilter.FilterValue(property.Name, oldValue);

                changes.Add(new PropertyChange(property.Name, filteredValue, null));
            }

            return changes;
        }

        /// <summary>
        /// Determines whether a property should be excluded from audit logging.
        /// Excludes navigation properties, computed columns, key properties, and system-managed timestamps.
        /// </summary>
        private static bool ShouldSkipProperty(string propertyName, EntityEntry entry)
        {
            // Skip system-managed timestamp properties
            if (propertyName is "UpdatedAt" or "CreatedAt" or "DeletedAt" or "IsDeleted")
                return true;

            var property = entry.Metadata.FindProperty(propertyName);
            //var navigation = entry.Metadata.FindNavigation(propertyName);

            if (property == null)
                return true;

            // Skip navigation properties
            if (entry.Metadata.FindNavigation(propertyName) != null)
            {
                return true;
            }

            // Skip shadow properties (not mapped to entity properties)
            if (property.IsShadowProperty())
                return true;

            // Skip computed columns (read-only properties)
            if (property.ValueGenerated == ValueGenerated.OnAddOrUpdate ||
                property.ValueGenerated == ValueGenerated.OnAdd)
                return true;

            return false;
        }

        /// <summary>
        /// Gets the entity ID as a string from the entry's key values.
        /// </summary>
        /// <param name="entry">Entity Framework change entry.</param>
        /// <returns>Entity ID as string, or empty string if unable to determine.</returns>
        public static string GetEntityId(EntityEntry entry)
        {
            var keyValues = entry.Metadata.FindPrimaryKey()?.Properties
                .Select(p => entry.CurrentValues[p])
                .ToList() ?? [];

            if (keyValues.Count == 0)
                return string.Empty;

            if (keyValues.Count == 1)
                return keyValues[0]?.ToString() ?? string.Empty;

            // Composite key
            return string.Join("|", keyValues.Select(v => v?.ToString() ?? string.Empty));
        }
    }
}
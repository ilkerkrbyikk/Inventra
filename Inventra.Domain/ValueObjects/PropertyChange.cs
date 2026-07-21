namespace Inventra.Domain.ValueObjects
{
    /// <summary>
    /// Represents a single property change in an audit log entry.
    /// Captures the property name, old value, and new value.
    /// </summary>
    public class PropertyChange
    {
        /// <summary>
        /// Name of the property that changed.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Previous value before the change. Null if creating a new entity.
        /// </summary>
        public string? OldValue { get; }

        /// <summary>
        /// New value after the change.
        /// </summary>
        public string? NewValue { get; }

        /// <summary>
        /// Initializes a new instance of the PropertyChange class.
        /// </summary>
        /// <param name="propertyName">Name of the changed property.</param>
        /// <param name="oldValue">Previous value before change.</param>
        /// <param name="newValue">New value after change.</param>
        public PropertyChange(string propertyName, string? oldValue, string? newValue)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException("Property name cannot be null or empty.", nameof(propertyName));

            PropertyName = propertyName;
            OldValue = oldValue;
            NewValue = newValue;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not PropertyChange other)
                return false;

            return PropertyName == other.PropertyName &&
                   OldValue == other.OldValue &&
                   NewValue == other.NewValue;
        }

        public override int GetHashCode()
            => HashCode.Combine(PropertyName, OldValue, NewValue);

        public override string ToString()
            => $"{PropertyName}: {OldValue ?? "null"} ? {NewValue ?? "null"}";
    }
}
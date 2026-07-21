using Inventra.Domain.Interfaces;

namespace Inventra.Domain.Entities
{
    /// <summary>
    /// Base class for all domain entities.
    /// Provides common properties for identity, timestamps, and soft delete support.
    /// </summary>
    public abstract class BaseEntity : IAuditable
    {
        /// <summary>
        /// Unique identifier for the entity.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Timestamp when the entity was created (UTC).
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Timestamp when the entity was last updated (UTC).
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Indicates whether the entity is soft deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Timestamp when the entity was soft deleted (UTC). Null if not deleted.
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// Initializes a new instance of the BaseEntity class.
        /// </summary>
        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            IsDeleted = false;
            DeletedAt = null;
        }

        /// <summary>
        /// Soft deletes the entity by setting IsDeleted to true and recording the deletion timestamp.
        /// </summary>
        public void SoftDelete()
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Restores a soft-deleted entity.
        /// </summary>
        public void Restore()
        {
            IsDeleted = false;
            DeletedAt = null;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
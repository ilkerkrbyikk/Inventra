namespace Inventra.Domain.Enums
{
    /// <summary>
    /// Represents the type of audit action performed on an entity.
    /// </summary>
    public enum AuditActionType
    {
        /// <summary>
        /// Entity was created.
        /// </summary>
        Create = 0,

        /// <summary>
        /// Entity was updated.
        /// </summary>
        Update = 1,

        /// <summary>
        /// Entity was deleted (soft or hard delete).
        /// </summary>
        Delete = 2
    }
}
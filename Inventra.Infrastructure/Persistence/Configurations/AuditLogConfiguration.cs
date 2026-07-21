using Inventra.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventra.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Entity Framework Core configuration for the AuditLog entity.
    /// Defines table structure, constraints, and indexes for optimal query performance.
    /// </summary>
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        /// <summary>
        /// Configures the AuditLog entity mapping to the database table.
        /// </summary>
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            // Table configuration
            builder.ToTable("AuditLogs");

            // Primary key
            builder.HasKey(a => a.Id);

            // Property configurations
            builder.Property(a => a.EntityType)
                .IsRequired()
                .HasMaxLength(500)
                .HasComment("Full type name of the audited entity");

            builder.Property(a => a.EntityId)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("Primary key value of the audited entity");

            builder.Property(a => a.Action)
                .IsRequired()
                .HasComment("Type of action: Create (0), Update (1), Delete (2)");

            builder.Property(a => a.ActionTimestamp)
                .IsRequired()
                .HasComment("Timestamp of the audit action in UTC");

            builder.Property(a => a.UserId)
                .HasMaxLength(255)
                .HasComment("ID of the user who performed the action");

            builder.Property(a => a.Username)
                .HasMaxLength(255)
                .HasComment("Username of the user who performed the action");

            builder.Property(a => a.IpAddress)
                .HasMaxLength(45)
                .HasComment("IP address from which the action was performed");

            builder.Property(a => a.ChangedProperties)
                .HasColumnType("nvarchar(max)")
                .HasComment("JSON-serialized array of property changes");

            // Audit log timestamps (inherited from BaseEntity)
            builder.Property(a => a.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()")
                .HasComment("Timestamp when the audit log was created");

            builder.Property(a => a.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()")
                .HasComment("Timestamp when the audit log was last updated");

            builder.Property(a => a.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false)
                .HasComment("Indicates whether this audit log is soft deleted");

            builder.Property(a => a.DeletedAt)
                .HasComment("Timestamp when this audit log was soft deleted");

            // Indexes for performance
            builder.HasIndex(a => a.EntityType)
                .HasName("IX_AuditLogs_EntityType");

            builder.HasIndex(a => new { a.EntityType, a.EntityId })
                .HasName("IX_AuditLogs_EntityType_EntityId");

            builder.HasIndex(a => a.ActionTimestamp)
                .HasName("IX_AuditLogs_ActionTimestamp");

            builder.HasIndex(a => a.UserId)
                .HasName("IX_AuditLogs_UserId");

            builder.HasIndex(a => a.Action)
                .HasName("IX_AuditLogs_Action");

            builder.HasIndex(a => a.IsDeleted)
                .HasName("IX_AuditLogs_IsDeleted");

            // Composite index for common query patterns
            builder.HasIndex(a => new { a.ActionTimestamp, a.IsDeleted })
                .HasName("IX_AuditLogs_ActionTimestamp_IsDeleted");
        }
    }
}
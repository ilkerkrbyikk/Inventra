using Inventra.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventra.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Entity Framework Core configuration for BranchInventoryRequest.
    /// Fluent API only — no Data Annotations on the entity class.
    /// </summary>
    public class BranchInventoryRequestConfiguration : IEntityTypeConfiguration<BranchInventoryRequest>
    {
        public void Configure(EntityTypeBuilder<BranchInventoryRequest> builder)
        {
            builder.ToTable("BranchInventoryRequests");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.BranchId)
                .IsRequired()
                .HasColumnName("BranchId");

            builder.Property(r => r.ProductId)
                .IsRequired()
                .HasColumnName("ProductId");

            builder.Property(r => r.WarehouseId)
                .IsRequired()
                .HasColumnName("WarehouseId");

            builder.Property(r => r.RequestedQuantity)
                .IsRequired()
                .HasColumnName("RequestedQuantity");

            builder.Property(r => r.Status)
                .IsRequired()
                .HasColumnName("Status");

            builder.Property(r => r.Notes)
                .HasMaxLength(500)
                .HasColumnName("Notes");

            builder.Property(r => r.ReviewNotes)
                .HasMaxLength(500)
                .HasColumnName("ReviewNotes");

            builder.Property(r => r.ReviewedByUserId)
                .HasMaxLength(255)
                .HasColumnName("ReviewedByUserId");

            builder.Property(r => r.ReviewedAt)
                .HasColumnName("ReviewedAt");

            builder.Property(r => r.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .HasColumnName("CreatedAt");

            builder.Property(r => r.UpdatedAt)
                .HasColumnName("UpdatedAt");

            builder.Property(r => r.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("IsDeleted");

            builder.Property(r => r.DeletedAt)
                .HasColumnName("DeletedAt");

            // Soft delete global query filter
            builder.HasQueryFilter(r => !r.IsDeleted);

            // Indexes for common query patterns
            builder.HasIndex(r => r.BranchId)
                .HasDatabaseName("IX_BranchInventoryRequests_BranchId");

            builder.HasIndex(r => r.ProductId)
                .HasDatabaseName("IX_BranchInventoryRequests_ProductId");

            builder.HasIndex(r => r.Status)
                .HasDatabaseName("IX_BranchInventoryRequests_Status");

            builder.HasIndex(r => new { r.BranchId, r.Status })
                .HasDatabaseName("IX_BranchInventoryRequests_BranchId_Status");
        }
    }
}

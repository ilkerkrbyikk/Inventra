using Inventra.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventra.Infrastructure.Persistence.Configurations
{
    public class ProcurementRecordConfiguration : IEntityTypeConfiguration<ProcurementRecord>
    {
        public void Configure(EntityTypeBuilder<ProcurementRecord> builder)
        {
            builder.ToTable("ProcurementRecords");

            builder.HasKey(pr => pr.Id);

            builder.Property(pr => pr.SupplierId)
                .IsRequired()
                .HasColumnName("SupplierId");

            builder.Property(pr => pr.ProductId)
                .IsRequired()
                .HasColumnName("ProductId");

            builder.Property(pr => pr.WarehouseId)
                .IsRequired()
                .HasColumnName("WarehouseId");

            builder.Property(pr => pr.Quantity)
                .IsRequired()
                .HasColumnName("Quantity");

            builder.Property(pr => pr.ProcurementDate)
                .IsRequired()
                .HasColumnName("ProcurementDate");

            builder.Property(pr => pr.UnitPrice)
                .HasPrecision(18, 2)
                .HasColumnName("UnitPrice");

            builder.Property(pr => pr.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("Status");

            builder.Property(pr => pr.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .HasColumnName("CreatedAt");

            builder.Property(pr => pr.UpdatedAt)
                .HasColumnName("UpdatedAt");

            builder.Property(pr => pr.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("IsDeleted");

            builder.Property(pr => pr.DeletedAt)
                .HasColumnName("DeletedAt");

            builder.HasQueryFilter(pr => !pr.IsDeleted);
        }
    }
}
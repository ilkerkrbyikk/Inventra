using Inventra.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventra.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("Name");

            builder.Property(p => p.Barcode)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("Barcode");

            builder.HasIndex(p => p.Barcode)
                .IsUnique();

            builder.Property(p => p.Price)
                .HasPrecision(18, 2)
                .HasColumnName("Price");

            builder.Property(p => p.StockQuantity)
                .IsRequired()
                .HasColumnName("StockQuantity");

            builder.Property(p => p.CriticalStockThreshold)
                .HasColumnName("CriticalStockThreshold")
                .HasComment("Stock level at or below which a low-stock notification is triggered. Null means no threshold is configured.");

            builder.Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .HasColumnName("CreatedAt");

            builder.Property(p => p.UpdatedAt)
                .HasColumnName("UpdatedAt");

            builder.Property(p => p.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("IsDeleted");

            builder.Property(p => p.DeletedAt)
                .HasColumnName("DeletedAt");

            builder.HasQueryFilter(p => !p.IsDeleted);
        }
    }
}

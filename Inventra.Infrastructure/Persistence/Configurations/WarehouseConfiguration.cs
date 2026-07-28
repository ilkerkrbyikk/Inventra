using Inventra.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventra.Infrastructure.Persistence.Configurations
{
    public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.ToTable("Warehouses");

            builder.HasKey(w => w.Id);

            builder.Property(w => w.Name)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("Name");

            builder.Property(w => w.Address)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnName("Address");

            builder.Property(w => w.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .HasColumnName("CreatedAt");

            builder.Property(w => w.UpdatedAt)
                .HasColumnName("UpdatedAt");

            builder.Property(w => w.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("IsDeleted");

            builder.Property(w => w.DeletedAt)
                .HasColumnName("DeletedAt");

            builder.HasQueryFilter(w => !w.IsDeleted);
        }
    }
}
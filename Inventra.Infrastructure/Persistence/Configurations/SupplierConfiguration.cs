using Inventra.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventra.Infrastructure.Persistence.Configurations
{
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.ToTable("Suppliers");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.FirmName)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("FirmName");

            builder.Property(s => s.ContactInfo)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("ContactInfo");

            builder.Property(s => s.AuthorizedPerson)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("AuthorizedPerson");

            builder.Property(s => s.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .HasColumnName("CreatedAt");

            builder.Property(s => s.UpdatedAt)
                .HasColumnName("UpdatedAt");

            builder.Property(s => s.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("IsDeleted");

            builder.Property(s => s.DeletedAt)
                .HasColumnName("DeletedAt");

            builder.HasQueryFilter(s => !s.IsDeleted);
        }
    }
}
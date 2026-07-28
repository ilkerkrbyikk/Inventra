using Inventra.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventra.Infrastructure.Persistence.Configurations
{
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.ToTable("Branches");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("Name");

            builder.Property(b => b.Address)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnName("Address");

            builder.Property(b => b.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .HasColumnName("CreatedAt");

            builder.Property(b => b.UpdatedAt)
                .HasColumnName("UpdatedAt");

            builder.Property(b => b.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("IsDeleted");

            builder.Property(b => b.DeletedAt)
                .HasColumnName("DeletedAt");

            builder.HasQueryFilter(b => !b.IsDeleted);
        }
    }
}
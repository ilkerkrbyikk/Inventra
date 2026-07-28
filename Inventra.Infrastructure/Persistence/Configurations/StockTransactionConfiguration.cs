using Inventra.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventra.Infrastructure.Persistence.Configurations
{
    public class StockTransactionConfiguration : IEntityTypeConfiguration<StockTransaction>
    {
        public void Configure(EntityTypeBuilder<StockTransaction> builder)
        {
            builder.ToTable("StockTransactions");

            builder.HasKey(st => st.Id);

            builder.Property(st => st.ProductId)
                .IsRequired()
                .HasColumnName("ProductId");

            builder.Property(st => st.FromWarehouseId)
                .IsRequired()
                .HasColumnName("FromWarehouseId");

            builder.Property(st => st.ToWarehouseId)
                .IsRequired()
                .HasColumnName("ToWarehouseId");

            builder.Property(st => st.RequestedQuantity)
                .IsRequired()
                .HasColumnName("RequestedQuantity");

            builder.Property(st => st.TransferredQuantity)
                .IsRequired()
                .HasDefaultValue(0)
                .HasColumnName("TransferredQuantity");

            builder.Property(st => st.DefectiveQuantity)
                .IsRequired()
                .HasDefaultValue(0)
                .HasColumnName("DefectiveQuantity");

            builder.Property(st => st.TransactionDate)
                .IsRequired()
                .HasColumnName("TransactionDate");

            builder.Property(st => st.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("Status");

            builder.Property(st => st.Notes)
                .HasMaxLength(500)
                .HasColumnName("Notes");

            builder.Property(st => st.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .HasColumnName("CreatedAt");

            builder.Property(st => st.UpdatedAt)
                .HasColumnName("UpdatedAt");

            builder.Property(st => st.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("IsDeleted");

            builder.Property(st => st.DeletedAt)
                .HasColumnName("DeletedAt");

            builder.HasQueryFilter(st => !st.IsDeleted);
        }
    }
}
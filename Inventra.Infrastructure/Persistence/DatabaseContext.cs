using System.Linq.Expressions;
using Inventra.Domain.Entities;
using Inventra.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Inventra.Infrastructure.Persistence
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<StockTransaction> StockTransactions { get; set; }
        public DbSet<ProcurementRecord> ProcurementRecords { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<BranchInventoryRequest> BranchInventoryRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all entity configurations from this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
        }
    }
}



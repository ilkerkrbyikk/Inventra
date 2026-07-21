using Microsoft.EntityFrameworkCore.Storage;

namespace Inventra.Application.Interfaces
{
    /// <summary>
    /// Unit of Work pattern implementation for coordinating repository operations.
    /// Ensures transactional consistency across multiple repositories.
    /// </summary>
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }
        //IBranchRepository Branches { get; }
        //IWarehouseRepository Warehouses { get; }
        //ISupplierRepository Suppliers { get; }
        IStockTransactionRepository StockTransactions { get; }
        IProcurementRepository Procurements { get; }
        IAuditLogRepository AuditLogs { get; }

        /// <summary>
        /// Saves all pending changes to the database asynchronously.
        /// </summary>
        /// <returns>Number of entities changed in the database.</returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Begins a new database transaction.
        /// </summary>
        /// <returns>A transaction object that can be committed or rolled back.</returns>
        Task<IDbContextTransaction> BeginTransactionAsync();

        /// <summary>
        /// Disposes all resources held by the unit of work.
        /// </summary>
        void Dispose();
    }
}
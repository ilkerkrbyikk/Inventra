namespace Inventra.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        IStockTransactionRepository StockTransactions { get; }
        IProcurementRepository Procurements { get; }
        IGenericRepository<Domain.Entities.Branch> Branches { get; }
        IGenericRepository<Domain.Entities.Warehouse> Warehouses { get; }
        IGenericRepository<Domain.Entities.Supplier> Suppliers { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
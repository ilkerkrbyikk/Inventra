using Inventra.Application.Interfaces;
using Inventra.Infrastructure.Persistence;
using Inventra.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Inventra.Infrastructure.UnitOfWork
{
    /// <summary>
    /// Unit of Work implementation for coordinating repository operations.
    /// Ensures transactional consistency across multiple repositories.
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DatabaseContext _context;

        private IProductRepository _productRepository;
        //private IBranchRepository _branchRepository;
        //private IWarehouseRepository _warehouseRepository;
        //private ISupplierRepository _supplierRepository;
        private IStockTransactionRepository _stockTransactionRepository;
        private IProcurementRepository _procurementRepository;
        private IAuditLogRepository _auditLogRepository;

        /// <summary>
        /// Initializes a new instance of the UnitOfWork class.
        /// </summary>
        /// <param name="context">Database context for repository operations.</param>
        public UnitOfWork(DatabaseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets the product repository, creating it lazily on first access.
        /// </summary>
        public IProductRepository Products
            => _productRepository ??= new ProductRepository(_context);

        /// <summary>
        /// Gets the branch repository, creating it lazily on first access.
        /// </summary>
        //public IBranchRepository Branches
        //    => _branchRepository ??= new BranchRepository(_context);

        /// <summary>
        /// Gets the warehouse repository, creating it lazily on first access.
        /// </summary>
        //public IWarehouseRepository Warehouses
        //    => _warehouseRepository ??= new WarehouseRepository(_context);

        /// <summary>
        /// Gets the supplier repository, creating it lazily on first access.
        /// </summary>
        //public ISupplierRepository Suppliers
        //    => _supplierRepository ??= new SupplierRepository(_context);

        /// <summary>
        /// Gets the stock transaction repository, creating it lazily on first access.
        /// </summary>
        public IStockTransactionRepository StockTransactions
            => _stockTransactionRepository ??= new StockTransactionRepository(_context);

        /// <summary>
        /// Gets the procurement repository, creating it lazily on first access.
        /// </summary>
        public IProcurementRepository Procurements
            => _procurementRepository ??= new ProcurementRepository(_context);

        /// <summary>
        /// Gets the audit log repository, creating it lazily on first access.
        /// </summary>
        public IAuditLogRepository AuditLogs
            => _auditLogRepository ??= new AuditLogRepository(_context);

        /// <summary>
        /// Saves all pending changes to the database asynchronously.
        /// Coordinates SaveChanges across all repositories.
        /// </summary>
        /// <returns>Number of entities changed in the database.</returns>
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Begins a new database transaction.
        /// </summary>
        /// <returns>A transaction object that can be committed or rolled back.</returns>
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        /// <summary>
        /// Disposes all resources held by the unit of work.
        /// </summary>
        public void Dispose()
        {
            _context?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
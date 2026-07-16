using Inventra.Application.Interfaces;
using Inventra.Domain.Entities;
using Inventra.Infrastructure.Persistence;
using Inventra.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Inventra.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;
        private IDbContextTransaction? _transaction;

        private IProductRepository? _productRepository;
        private IStockTransactionRepository? _stockTransactionRepository;
        private IProcurementRepository? _procurementRepository;
        private IGenericRepository<Branch>? _branchRepository;
        private IGenericRepository<Warehouse>? _warehouseRepository;
        private IGenericRepository<Supplier>? _supplierRepository;

        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
        }

        public IProductRepository Products
            => _productRepository ??= new ProductRepository(_context);

        public IStockTransactionRepository StockTransactions
            => _stockTransactionRepository ??= new StockTransactionRepository(_context);

        public IProcurementRepository Procurements
            => _procurementRepository ??= new ProcurementRepository(_context);

        public IGenericRepository<Branch> Branches
            => _branchRepository ??= new GenericRepository<Branch>(_context);

        public IGenericRepository<Warehouse> Warehouses
            => _warehouseRepository ??= new GenericRepository<Warehouse>(_context);

        public IGenericRepository<Supplier> Suppliers
            => _supplierRepository ??= new GenericRepository<Supplier>(_context);

        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();

        public async Task BeginTransactionAsync()
            => _transaction = await _context.Database.BeginTransactionAsync();

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync();
                await _transaction?.CommitAsync()!;
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                await _transaction?.DisposeAsync()!;
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            try
            {
                await _transaction?.RollbackAsync()!;
            }
            finally
            {
                await _transaction?.DisposeAsync()!;
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
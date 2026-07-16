using Inventra.Application.Interfaces;
using Inventra.Domain.Entities;
using Inventra.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventra.Infrastructure.Repositories
{
    public class StockTransactionRepository : GenericRepository<StockTransaction>, IStockTransactionRepository
    {
        public StockTransactionRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<IEnumerable<StockTransaction>> GetByStatusAsync(string status)
            => await _context.StockTransactions.Where(t => t.Status == status).ToListAsync();

        public async Task<IEnumerable<StockTransaction>> GetPendingTransfersAsync(Guid warehouseId)
            => await _context.StockTransactions
                .Where(t => t.ToWarehouseId == warehouseId && t.Status == "Pending")
                .ToListAsync();
    }
}
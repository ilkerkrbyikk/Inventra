using Inventra.Domain.Entities;

namespace Inventra.Application.Interfaces
{
    public interface IStockTransactionRepository : IGenericRepository<StockTransaction>
    {
        Task<IEnumerable<StockTransaction>> GetByStatusAsync(string status);
        Task<IEnumerable<StockTransaction>> GetPendingTransfersAsync(Guid warehouseId);
    }
}
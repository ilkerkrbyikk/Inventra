using Inventra.Domain.Entities;

namespace Inventra.Application.Interfaces
{
    public interface IProcurementRepository : IGenericRepository<ProcurementRecord>
    {
        Task<IEnumerable<ProcurementRecord>> GetByWarehouseAsync(Guid warehouseId);
    }
}
using Inventra.Application.Interfaces;
using Inventra.Domain.Entities;
using Inventra.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventra.Infrastructure.Repositories
{
    public class ProcurementRepository : GenericRepository<ProcurementRecord>, IProcurementRepository
    {
        public ProcurementRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ProcurementRecord>> GetByWarehouseAsync(Guid warehouseId)
            => await _context.ProcurementRecords.Where(p => p.WarehouseId == warehouseId).ToListAsync();
    }
}
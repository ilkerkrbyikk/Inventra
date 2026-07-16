using Inventra.Application.Common.CQRS;

namespace Inventra.Application.Features.Procurement.Commands
{
    public class CreateProcurementCommand : ICommand<Guid>
    {
        public Guid SupplierId { get; set; }
        public Guid ProductId { get; set; }
        public Guid WarehouseId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
namespace Inventra.Application.DTOs
{
    public class ProcurementCreateDto
    {
        public Guid SupplierId { get; set; }
        public Guid ProductId { get; set; }
        public Guid WarehouseId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
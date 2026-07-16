namespace Inventra.Domain.Entities
{
    public class ProcurementRecord : BaseEntity
    {
        public Guid SupplierId { get; set; }
        public Guid ProductId { get; set; }
        public Guid WarehouseId { get; set; }
        public int Quantity { get; set; }
        public DateTime ProcurementDate { get; set; }
        public decimal UnitPrice { get; set; }
        public string Status { get; set; }
    }
}
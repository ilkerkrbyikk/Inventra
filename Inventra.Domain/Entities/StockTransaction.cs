namespace Inventra.Domain.Entities
{
    public class StockTransaction : BaseEntity
    {
        public Guid ProductId { get; set; }
        public Guid FromWarehouseId { get; set; }
        public Guid ToWarehouseId { get; set; }
        public int RequestedQuantity { get; set; }
        public int TransferredQuantity { get; set; }
        public int DefectiveQuantity { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
    }
}
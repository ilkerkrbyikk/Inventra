namespace Inventra.Application.Features.StockTransfer.DTOs
{
    public class StockTransactionDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid FromWarehouseId { get; set; }
        public Guid ToWarehouseId { get; set; }
        public int RequestedQuantity { get; set; }
        public int TransferredQuantity { get; set; }
        public int DefectiveQuantity { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
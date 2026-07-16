namespace Inventra.Application.DTOs
{
    public class StockTransferCompleteDto
    {
        public Guid TransactionId { get; set; }
        public int TransferredQuantity { get; set; }
        public int DefectiveQuantity { get; set; }
        public string Notes { get; set; }
    }
}
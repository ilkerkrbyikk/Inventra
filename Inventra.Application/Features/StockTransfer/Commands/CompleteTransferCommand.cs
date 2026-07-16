using Inventra.Application.Common.CQRS;

namespace Inventra.Application.Features.StockTransfer.Commands
{
    public class CompleteTransferCommand : ICommand
    {
        public Guid TransactionId { get; set; }
        public int TransferredQuantity { get; set; }
        public int DefectiveQuantity { get; set; }
        public string? Notes { get; set; }
    }
}
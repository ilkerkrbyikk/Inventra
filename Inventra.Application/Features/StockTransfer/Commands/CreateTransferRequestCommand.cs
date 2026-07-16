using Inventra.Application.Common.CQRS;

namespace Inventra.Application.Features.StockTransfer.Commands
{
    public class CreateTransferRequestCommand : ICommand<Guid>
    {
        public Guid ProductId { get; set; }
        public Guid FromWarehouseId { get; set; }
        public Guid ToWarehouseId { get; set; }
        public int Quantity { get; set; }
    }
}
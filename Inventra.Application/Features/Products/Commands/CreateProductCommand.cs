using Inventra.Application.Common.CQRS;

namespace Inventra.Application.Features.Products.Commands
{
    /// <summary>
    /// Command to create a new product.
    /// Returns the ID of the created product.
    /// </summary>
    public record CreateProductCommand(
        string Name,
        string Barcode,
        decimal Price,
        int StockQuantity) : ICommand<Guid>;
}
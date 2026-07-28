using Inventra.Application.Common.CQRS;

namespace Inventra.Application.Features.Products.Commands
{
    /// <summary>
    /// Command to update an existing product.
    /// Null values in the DTO are ignored (only provided fields are updated).
    /// </summary>
    public record UpdateProductCommand(
        Guid Id,
        string? Name,
        decimal? Price,
        int? StockQuantity,
        int? CriticalStockThreshold) : ICommand;
}

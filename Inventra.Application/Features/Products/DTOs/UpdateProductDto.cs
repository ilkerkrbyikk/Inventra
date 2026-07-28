namespace Inventra.Application.Features.Products.DTOs
{
    /// <summary>
    /// Data Transfer Object for updating an existing product.
    /// Represents the input from the API request.
    /// </summary>
    public record UpdateProductDto(
        string? Name,
        decimal? Price,
        int? StockQuantity);
}
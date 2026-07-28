namespace Inventra.Application.Features.Products.DTOs
{
    /// <summary>
    /// Data Transfer Object for Product entity.
    /// Used for returning product data in API responses.
    /// </summary>
    public record ProductDto(
        Guid Id,
        string Name,
        string Barcode,
        decimal Price,
        int StockQuantity,
        DateTime CreatedAt,
        DateTime? UpdatedAt);
}
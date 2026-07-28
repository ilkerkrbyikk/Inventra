namespace Inventra.Application.Features.Products.DTOs
{
    /// <summary>
    /// Data Transfer Object for creating a new product.
    /// Represents the input from the API request.
    /// </summary>
    public record CreateProductDto(
        string Name,
        string Barcode,
        decimal Price,
        int StockQuantity);
}
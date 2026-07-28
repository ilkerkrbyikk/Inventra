namespace Inventra.Application.Features.Suppliers.DTOs
{
    /// <summary>
    /// Data Transfer Object for Supplier entity.
    /// Used for returning supplier data in API responses.
    /// </summary>
    public record SupplierDto(
        Guid Id,
        string FirmName,
        string ContactInfo,
        string AuthorizedPerson,
        DateTime CreatedAt,
        DateTime? UpdatedAt);
}
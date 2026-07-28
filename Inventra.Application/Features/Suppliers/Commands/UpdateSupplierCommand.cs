using Inventra.Application.Common.CQRS;

namespace Inventra.Application.Features.Suppliers.Commands
{
    /// <summary>
    /// Command to update an existing supplier.
    /// Null values in the command are ignored (only provided fields are updated).
    /// </summary>
    public record UpdateSupplierCommand(
        Guid Id,
        string? FirmName,
        string? ContactInfo,
        string? AuthorizedPerson) : ICommand;
}

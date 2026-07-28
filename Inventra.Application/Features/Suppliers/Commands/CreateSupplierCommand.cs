using Inventra.Application.Common.CQRS;

namespace Inventra.Application.Features.Suppliers.Commands
{
    /// <summary>
    /// Command to create a new supplier.
    /// Returns the ID of the created supplier.
    /// </summary>
    public record CreateSupplierCommand(
        string FirmName,
        string ContactInfo,
        string AuthorizedPerson) : ICommand<Guid>;
}
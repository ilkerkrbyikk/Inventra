using Inventra.Application.Common.CQRS;

namespace Inventra.Application.Features.Suppliers.Commands
{
    /// <summary>
    /// Command to delete (soft delete) a supplier.
    /// The supplier is marked as deleted but not removed from the database.
    /// </summary>
    public record DeleteSupplierCommand(Guid Id) : ICommand;
}

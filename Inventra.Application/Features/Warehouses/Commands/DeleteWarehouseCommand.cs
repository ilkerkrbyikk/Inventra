using Inventra.Application.Common.CQRS;

namespace Inventra.Application.Features.Warehouses.Commands
{
    /// <summary>
    /// Command to delete (soft delete) a warehouse.
    /// The warehouse is marked as deleted but not removed from the database.
    /// </summary>
    public record DeleteWarehouseCommand(Guid Id) : ICommand;
}

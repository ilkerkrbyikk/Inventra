using Inventra.Application.Common.CQRS;

namespace Inventra.Application.Features.Warehouses.Commands
{
    /// <summary>
    /// Command to create a new warehouse.
    /// Returns the ID of the created warehouse.
    /// </summary>
    public record CreateWarehouseCommand(
        string Name,
        string Address) : ICommand<Guid>;
}

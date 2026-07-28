using Inventra.Application.Common.CQRS;

namespace Inventra.Application.Features.Warehouses.Commands
{
    /// <summary>
    /// Command to update an existing warehouse.
    /// Null values in the command are ignored (only provided fields are updated).
    /// </summary>
    public record UpdateWarehouseCommand(
        Guid Id,
        string? Name,
        string? Address) : ICommand;
}

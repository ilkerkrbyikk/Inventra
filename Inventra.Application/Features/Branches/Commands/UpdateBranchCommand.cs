using Inventra.Application.Common.CQRS;

namespace Inventra.Application.Features.Branches.Commands
{
    /// <summary>
    /// Command to update an existing branch.
    /// Null values in the command are ignored (only provided fields are updated).
    /// </summary>
    public record UpdateBranchCommand(
        Guid Id,
        string? Name,
        string? Address) : ICommand;
}

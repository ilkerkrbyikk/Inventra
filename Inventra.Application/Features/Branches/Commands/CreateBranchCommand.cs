using Inventra.Application.Common.CQRS;

namespace Inventra.Application.Features.Branches.Commands
{
    /// <summary>
    /// Command to create a new branch.
    /// Returns the ID of the created branch.
    /// </summary>
    public record CreateBranchCommand(
        string Name,
        string Address) : ICommand<Guid>;
}

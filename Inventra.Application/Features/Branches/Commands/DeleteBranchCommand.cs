using Inventra.Application.Common.CQRS;

namespace Inventra.Application.Features.Branches.Commands
{
    /// <summary>
    /// Command to delete (soft delete) a branch.
    /// The branch is marked as deleted but not removed from the database.
    /// </summary>
    public record DeleteBranchCommand(Guid Id) : ICommand;
}

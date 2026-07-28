using Inventra.Application.Common.CQRS;
using Inventra.Application.Features.Branches.DTOs;

namespace Inventra.Application.Features.Branches.Queries
{
    /// <summary>
    /// Query to retrieve a branch by its ID.
    /// </summary>
    public record GetBranchByIdQuery(Guid Id) : IQuery<BranchDto>;
}

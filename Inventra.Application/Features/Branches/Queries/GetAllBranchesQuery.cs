using Inventra.Application.Common.CQRS;
using Inventra.Application.Features.Branches.DTOs;

namespace Inventra.Application.Features.Branches.Queries
{
    /// <summary>
    /// Query to retrieve all branches.
    /// </summary>
    public record GetAllBranchesQuery(int PageNumber = 1, int PageSize = 100) : IQuery<IEnumerable<BranchDto>>;
}

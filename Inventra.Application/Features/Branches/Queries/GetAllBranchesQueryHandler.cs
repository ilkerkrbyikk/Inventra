using Inventra.Application.Common.CQRS;
using Inventra.Application.Common.Results;
using Inventra.Application.Features.Branches.DTOs;
using Inventra.Application.Interfaces;
using Inventra.Domain.Entities;

namespace Inventra.Application.Features.Branches.Queries
{
    /// <summary>
    /// Handler for GetAllBranchesQuery.
    /// </summary>
    public class GetAllBranchesQueryHandler : IQueryHandler<GetAllBranchesQuery, IEnumerable<BranchDto>>
    {
        private readonly IGenericRepository<Branch> _branchRepository;

        public GetAllBranchesQueryHandler(IGenericRepository<Branch> branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task<Result<IEnumerable<BranchDto>>> Handle(
            GetAllBranchesQuery request,
            CancellationToken cancellationToken)
        {
            var branches = await _branchRepository.GetAllAsync(cancellationToken);

            var branchDtos = branches
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(b => new BranchDto(
                    b.Id,
                    b.Name,
                    b.Address,
                    b.CreatedAt,
                    b.UpdatedAt))
                .ToList();

            return Result.Success<IEnumerable<BranchDto>>(
                branchDtos,
                $"Retrieved {branchDtos.Count} branches.");
        }
    }
}

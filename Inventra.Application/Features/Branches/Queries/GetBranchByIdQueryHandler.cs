using Inventra.Application.Common.CQRS;
using Inventra.Application.Common.Results;
using Inventra.Application.Features.Branches.DTOs;
using Inventra.Application.Interfaces;
using Inventra.Domain.Entities;

namespace Inventra.Application.Features.Branches.Queries
{
    /// <summary>
    /// Handler for GetBranchByIdQuery.
    /// Retrieves a branch by ID and maps it to BranchDto.
    /// </summary>
    public class GetBranchByIdQueryHandler : IQueryHandler<GetBranchByIdQuery, BranchDto>
    {
        private readonly IGenericRepository<Branch> _branchRepository;

        public GetBranchByIdQueryHandler(IGenericRepository<Branch> branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task<Result<BranchDto>> Handle(
            GetBranchByIdQuery request,
            CancellationToken cancellationToken)
        {
            var branch = await _branchRepository.GetByIdAsync(request.Id, cancellationToken);

            if (branch is null)
                return Result.Failure<BranchDto>("Branch not found.");

            var branchDto = new BranchDto(
                branch.Id,
                branch.Name,
                branch.Address,
                branch.CreatedAt,
                branch.UpdatedAt);

            return Result.Success(branchDto, "Branch retrieved successfully.");
        }
    }
}

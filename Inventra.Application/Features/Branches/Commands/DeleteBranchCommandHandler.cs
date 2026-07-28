using Inventra.Application.Common.CQRS;
using Inventra.Application.Common.Results;
using Inventra.Application.Interfaces;
using Inventra.Domain.Entities;

namespace Inventra.Application.Features.Branches.Commands
{
    /// <summary>
    /// Handler for DeleteBranchCommand.
    /// Performs a soft delete by marking the branch as deleted.
    /// Validation is performed by the ValidationBehavior pipeline.
    /// </summary>
    public class DeleteBranchCommandHandler : ICommandHandler<DeleteBranchCommand>
    {
        private readonly IGenericRepository<Branch> _branchRepository;

        public DeleteBranchCommandHandler(IGenericRepository<Branch> branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task<Result> Handle(
            DeleteBranchCommand request,
            CancellationToken cancellationToken)
        {
            // Validation already done by ValidationBehavior
            var branch = await _branchRepository.GetByIdAsync(request.Id, cancellationToken);

            if (branch is null)
                return Result.Failure("Branch not found.");

            // Soft delete
            branch.IsDeleted = true;
            branch.DeletedAt = DateTime.UtcNow;
            branch.UpdatedAt = DateTime.UtcNow;

            await _branchRepository.UpdateAsync(branch, cancellationToken);

            return Result.Success("Branch deleted successfully.");
        }
    }
}

using Inventra.Application.Common.CQRS;
using Inventra.Application.Common.Results;
using Inventra.Application.Interfaces;
using Inventra.Domain.Entities;

namespace Inventra.Application.Features.Branches.Commands
{
    /// <summary>
    /// Handler for UpdateBranchCommand.
    /// Updates an existing branch with provided fields only (null fields are ignored).
    /// Validation is performed by the ValidationBehavior pipeline.
    /// </summary>
    public class UpdateBranchCommandHandler : ICommandHandler<UpdateBranchCommand>
    {
        private readonly IGenericRepository<Branch> _branchRepository;

        public UpdateBranchCommandHandler(IGenericRepository<Branch> branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task<Result> Handle(
            UpdateBranchCommand request,
            CancellationToken cancellationToken)
        {
            // Validation already done by ValidationBehavior
            var branch = await _branchRepository.GetByIdAsync(request.Id, cancellationToken);

            if (branch is null)
                return Result.Failure("Branch not found.");

            // Update only provided fields
            if (!string.IsNullOrEmpty(request.Name))
                branch.Name = request.Name;

            if (!string.IsNullOrEmpty(request.Address))
                branch.Address = request.Address;

            branch.UpdatedAt = DateTime.UtcNow;

            await _branchRepository.UpdateAsync(branch, cancellationToken);

            return Result.Success("Branch updated successfully.");
        }
    }
}

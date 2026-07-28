using Inventra.Application.Common.CQRS;
using Inventra.Application.Common.Results;
using Inventra.Application.Interfaces;
using Inventra.Domain.Entities;

namespace Inventra.Application.Features.Branches.Commands
{
    /// <summary>
    /// Handler for CreateBranchCommand.
    /// Creates a new branch and returns its ID.
    /// Validation is performed by the ValidationBehavior pipeline.
    /// </summary>
    public class CreateBranchCommandHandler : ICommandHandler<CreateBranchCommand, Guid>
    {
        private readonly IGenericRepository<Branch> _branchRepository;

        public CreateBranchCommandHandler(IGenericRepository<Branch> branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task<Result<Guid>> Handle(
            CreateBranchCommand request,
            CancellationToken cancellationToken)
        {
            // Validation already done by ValidationBehavior
            // Create new branch
            var branch = new Branch
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Address = request.Address,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            await _branchRepository.AddAsync(branch, cancellationToken);

            return Result.Success(branch.Id, "Branch created successfully.");
        }
    }
}

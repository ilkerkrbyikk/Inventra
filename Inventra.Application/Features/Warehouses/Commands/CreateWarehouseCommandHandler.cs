using Inventra.Application.Common.CQRS;
using Inventra.Application.Common.Results;
using Inventra.Application.Interfaces;
using Inventra.Domain.Entities;

namespace Inventra.Application.Features.Warehouses.Commands
{
    /// <summary>
    /// Handler for CreateWarehouseCommand.
    /// Creates a new warehouse and returns its ID.
    /// Validation is performed by the ValidationBehavior pipeline.
    /// </summary>
    public class CreateWarehouseCommandHandler : ICommandHandler<CreateWarehouseCommand, Guid>
    {
        private readonly IGenericRepository<Warehouse> _warehouseRepository;

        public CreateWarehouseCommandHandler(IGenericRepository<Warehouse> warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        public async Task<Result<Guid>> Handle(
            CreateWarehouseCommand request,
            CancellationToken cancellationToken)
        {
            // Validation already done by ValidationBehavior
            // Create new warehouse
            var warehouse = new Warehouse
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Address = request.Address,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            await _warehouseRepository.AddAsync(warehouse, cancellationToken);

            return Result.Success(warehouse.Id, "Warehouse created successfully.");
        }
    }
}

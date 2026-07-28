using Inventra.Application.Common.CQRS;
using Inventra.Application.Common.Results;
using Inventra.Application.Interfaces;
using Inventra.Domain.Entities;

namespace Inventra.Application.Features.Warehouses.Commands
{
    /// <summary>
    /// Handler for UpdateWarehouseCommand.
    /// Updates an existing warehouse with provided fields only (null fields are ignored).
    /// Validation is performed by the ValidationBehavior pipeline.
    /// </summary>
    public class UpdateWarehouseCommandHandler : ICommandHandler<UpdateWarehouseCommand>
    {
        private readonly IGenericRepository<Warehouse> _warehouseRepository;

        public UpdateWarehouseCommandHandler(IGenericRepository<Warehouse> warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        public async Task<Result> Handle(
            UpdateWarehouseCommand request,
            CancellationToken cancellationToken)
        {
            // Validation already done by ValidationBehavior
            var warehouse = await _warehouseRepository.GetByIdAsync(request.Id, cancellationToken);

            if (warehouse is null)
                return Result.Failure("Warehouse not found.");

            // Update only provided fields
            if (!string.IsNullOrEmpty(request.Name))
                warehouse.Name = request.Name;

            if (!string.IsNullOrEmpty(request.Address))
                warehouse.Address = request.Address;

            warehouse.UpdatedAt = DateTime.UtcNow;

            await _warehouseRepository.UpdateAsync(warehouse, cancellationToken);

            return Result.Success("Warehouse updated successfully.");
        }
    }
}

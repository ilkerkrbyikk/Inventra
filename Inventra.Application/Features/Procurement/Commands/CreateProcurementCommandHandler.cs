using Inventra.Application.Common.CQRS;
using Inventra.Application.Common.Results;
using Inventra.Application.Common.Validation;
using Inventra.Application.Interfaces;
using Inventra.Domain.Entities;
using FluentValidation;

namespace Inventra.Application.Features.Procurement.Commands
{
    public class CreateProcurementCommandHandler : ICommandHandler<CreateProcurementCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateProcurementCommand> _validator;

        public CreateProcurementCommandHandler(IUnitOfWork unitOfWork, IValidator<CreateProcurementCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<Result<Guid>> Handle(CreateProcurementCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                return Result.Failure<Guid>(validationResult.GetErrorMessages());

            var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId);
            if (product == null)
                return Result.Failure<Guid>("Product not found.");

            var procurement = new ProcurementRecord
            {
                Id = Guid.NewGuid(),
                SupplierId = request.SupplierId,
                ProductId = request.ProductId,
                WarehouseId = request.WarehouseId,
                Quantity = request.Quantity,
                UnitPrice = request.UnitPrice,
                ProcurementDate = DateTime.UtcNow,
                Status = "Completed"
            };

            product.StockQuantity += request.Quantity;

            await _unitOfWork.Procurements.AddAsync(procurement);
            await _unitOfWork.Products.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success(procurement.Id, "Procurement created successfully.");
        }
    }
}
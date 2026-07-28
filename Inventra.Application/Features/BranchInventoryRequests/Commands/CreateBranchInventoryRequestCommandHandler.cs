using Inventra.Application.Common.CQRS;
using Inventra.Application.Common.Notifications;
using Inventra.Application.Common.Options;
using Inventra.Application.Common.Results;
using Inventra.Application.Interfaces;
using Inventra.Domain.Entities;
using Inventra.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Options;

namespace Inventra.Application.Features.BranchInventoryRequests.Commands
{
    /// <summary>
    /// Handler for CreateBranchInventoryRequestCommand.
    ///
    /// Responsibilities:
    /// 1. Validate that the referenced branch, product, and warehouse exist.
    /// 2. Persist the new BranchInventoryRequest with Pending status.
    /// 3. If the requested quantity meets or exceeds LargeRequestThreshold,
    ///    publish a LargeInventoryRequestedNotification via MediatR.
    ///    The notification handler (Infrastructure) will push a SignalR message
    ///    to the warehouse manager — this handler does not know about SignalR.
    ///
    /// Validation is performed by ValidationBehavior before this handler runs.
    /// </summary>
    public class CreateBranchInventoryRequestCommandHandler
        : ICommandHandler<CreateBranchInventoryRequestCommand, Guid>
    {
        private readonly IGenericRepository<BranchInventoryRequest> _requestRepository;
        private readonly IGenericRepository<Branch> _branchRepository;
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<Warehouse> _warehouseRepository;
        private readonly IPublisher _publisher;
        private readonly InventoryNotificationOptions _notificationOptions;

        public CreateBranchInventoryRequestCommandHandler(
            IGenericRepository<BranchInventoryRequest> requestRepository,
            IGenericRepository<Branch> branchRepository,
            IGenericRepository<Product> productRepository,
            IGenericRepository<Warehouse> warehouseRepository,
            IPublisher publisher,
            IOptions<InventoryNotificationOptions> notificationOptions)
        {
            _requestRepository = requestRepository;
            _branchRepository = branchRepository;
            _productRepository = productRepository;
            _warehouseRepository = warehouseRepository;
            _publisher = publisher;
            _notificationOptions = notificationOptions.Value;
        }

        public async Task<Result<Guid>> Handle(
            CreateBranchInventoryRequestCommand request,
            CancellationToken cancellationToken)
        {
            // --- Existence checks ---
            var branch = await _branchRepository.GetByIdAsync(request.BranchId, cancellationToken);
            if (branch is null)
                return Result.Failure<Guid>("Branch not found.");

            var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
            if (product is null)
                return Result.Failure<Guid>("Product not found.");

            var warehouse = await _warehouseRepository.GetByIdAsync(request.WarehouseId, cancellationToken);
            if (warehouse is null)
                return Result.Failure<Guid>("Warehouse not found.");

            // --- Persist the request ---
            var inventoryRequest = new BranchInventoryRequest
            {
                Id = Guid.NewGuid(),
                BranchId = request.BranchId,
                ProductId = request.ProductId,
                WarehouseId = request.WarehouseId,
                RequestedQuantity = request.RequestedQuantity,
                Status = BranchInventoryRequestStatus.Pending,
                Notes = request.Notes,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            await _requestRepository.AddAsync(inventoryRequest, cancellationToken);

            // --- Large request notification ---
            // Business rule: if the quantity meets or exceeds the threshold,
            // the warehouse manager must be notified immediately.
            // We publish an application event — transport details are in Infrastructure.
            if (request.RequestedQuantity >= _notificationOptions.LargeRequestThreshold)
            {
                var notification = new LargeInventoryRequestedNotification(
                    RequestId: inventoryRequest.Id,
                    BranchId: branch.Id,
                    BranchName: branch.Name,
                    ProductId: product.Id,
                    ProductName: product.Name,
                    RequestedQuantity: request.RequestedQuantity,
                    LargeRequestThreshold: _notificationOptions.LargeRequestThreshold,
                    WarehouseManagerUserId: request.WarehouseManagerUserId);

                await _publisher.Publish(notification, cancellationToken);
            }

            return Result.Success(inventoryRequest.Id, "Branch inventory request created successfully.");
        }
    }
}

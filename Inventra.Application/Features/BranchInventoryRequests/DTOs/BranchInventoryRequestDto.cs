using Inventra.Domain.Enums;

namespace Inventra.Application.Features.BranchInventoryRequests.DTOs
{
    /// <summary>
    /// Data Transfer Object for BranchInventoryRequest.
    /// Used for returning request data in API responses.
    /// </summary>
    public record BranchInventoryRequestDto(
        Guid Id,
        Guid BranchId,
        string BranchName,
        Guid ProductId,
        string ProductName,
        Guid WarehouseId,
        int RequestedQuantity,
        BranchInventoryRequestStatus Status,
        string? Notes,
        string? ReviewNotes,
        string? ReviewedByUserId,
        DateTime? ReviewedAt,
        DateTime CreatedAt,
        DateTime? UpdatedAt);
}

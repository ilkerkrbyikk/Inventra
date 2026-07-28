using Inventra.Domain.Enums;

namespace Inventra.Domain.Entities
{
    /// <summary>
    /// Represents a stock request submitted by a branch to the warehouse.
    /// When the requested quantity exceeds the configured large-request threshold,
    /// a real-time notification is dispatched to the warehouse manager.
    /// </summary>
    public class BranchInventoryRequest : BaseEntity
    {
        /// <summary>
        /// The branch that submitted this request.
        /// </summary>
        public Guid BranchId { get; set; }

        /// <summary>
        /// The product being requested.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// The warehouse from which the stock is being requested.
        /// </summary>
        public Guid WarehouseId { get; set; }

        /// <summary>
        /// Number of units requested by the branch.
        /// </summary>
        public int RequestedQuantity { get; set; }

        /// <summary>
        /// Current lifecycle status of this request.
        /// </summary>
        public BranchInventoryRequestStatus Status { get; set; }

        /// <summary>
        /// Optional note from the branch manager explaining the urgency or context.
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Optional note from the warehouse manager when approving or rejecting.
        /// </summary>
        public string? ReviewNotes { get; set; }

        /// <summary>
        /// The user ID of the warehouse manager who reviewed this request.
        /// Null until the request is reviewed.
        /// </summary>
        public string? ReviewedByUserId { get; set; }

        /// <summary>
        /// Timestamp when the request was reviewed. Null until reviewed.
        /// </summary>
        public DateTime? ReviewedAt { get; set; }
    }
}

namespace Inventra.Domain.Enums
{
    /// <summary>
    /// Represents the lifecycle status of a branch inventory request.
    /// </summary>
    public enum BranchInventoryRequestStatus
    {
        /// <summary>
        /// Request has been submitted and is awaiting warehouse manager review.
        /// </summary>
        Pending = 0,

        /// <summary>
        /// Request has been approved by the warehouse manager.
        /// </summary>
        Approved = 1,

        /// <summary>
        /// Request has been rejected by the warehouse manager.
        /// </summary>
        Rejected = 2,

        /// <summary>
        /// Request has been fulfilled — stock has been dispatched to the branch.
        /// </summary>
        Fulfilled = 3
    }
}

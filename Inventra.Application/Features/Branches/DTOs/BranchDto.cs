namespace Inventra.Application.Features.Branches.DTOs
{
    /// <summary>
    /// Data Transfer Object for Branch entity.
    /// Used for returning branch data in API responses.
    /// </summary>
    public record BranchDto(
        Guid Id,
        string Name,
        string Address,
        DateTime CreatedAt,
        DateTime? UpdatedAt);
}

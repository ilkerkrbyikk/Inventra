using Inventra.Application.Common.CQRS;

namespace Inventra.Application.Features.Products.Commands
{
    /// <summary>
    /// Command to delete (soft delete) a product.
    /// The product is marked as deleted but not removed from the database.
    /// </summary>
    public record DeleteProductCommand(Guid Id) : ICommand;
}
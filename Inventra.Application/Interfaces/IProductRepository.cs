using Inventra.Domain.Entities;

namespace Inventra.Application.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product?> GetByBarcodeAsync(string barcode);
    }
}
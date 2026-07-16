using Inventra.Application.DTOs;
using Inventra.Application.Interfaces;
using Inventra.Domain.Entities;

namespace Inventra.Application.Services
{
    public class ProcurementService
    {
        private readonly IProcurementRepository _procurementRepository;
        private readonly IProductRepository _productRepository;

        public ProcurementService(IProcurementRepository procurementRepository, IProductRepository productRepository)
        {
            _procurementRepository = procurementRepository;
            _productRepository = productRepository;
        }

        public async Task<ProcurementRecord> CreateProcurementAsync(ProcurementCreateDto dto)
        {
            var product = await _productRepository.GetByIdAsync(dto.ProductId);
            if (product == null)
                throw new ArgumentException("Product not found.");

            var procurement = new ProcurementRecord
            {
                Id = Guid.NewGuid(),
                SupplierId = dto.SupplierId,
                ProductId = dto.ProductId,
                WarehouseId = dto.WarehouseId,
                Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice,
                ProcurementDate = DateTime.UtcNow,
                Status = "Completed"
            };

            product.StockQuantity += dto.Quantity;

            await _procurementRepository.AddAsync(procurement);
            await _productRepository.UpdateAsync(product);

            return procurement;
        }
    }
}
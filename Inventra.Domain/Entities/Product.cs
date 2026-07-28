namespace Inventra.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Barcode { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int? CriticalStockThreshold { get; set; }
    }
}

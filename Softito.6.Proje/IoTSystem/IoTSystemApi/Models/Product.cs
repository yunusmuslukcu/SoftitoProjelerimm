namespace IoTSystemApi.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public int UnitPrice { get; set; }
        public int StockQuantity { get; set; }
        public DateTime CreatedDate { get; set; }

        public int? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
    }
}

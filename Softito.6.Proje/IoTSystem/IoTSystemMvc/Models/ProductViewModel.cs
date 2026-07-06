namespace IoTSystemMvc.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; } 
        public string ProductCode { get; set; } 
        public int UnitPrice { get; set; }
        public int StockQuantity { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

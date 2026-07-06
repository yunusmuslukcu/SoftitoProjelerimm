namespace Finansal.PortfoyProject.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public string StockCode { get; set; }
        public string StockName { get; set; }
        public int Quantity { get; set; }
        public decimal AvgPrice { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}

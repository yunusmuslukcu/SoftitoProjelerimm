namespace DapperProjesi.Models
{
    public class Orders
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int WalletId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;


        public string? CustomerName { get; set; }
        public string? ProductName { get; set; }
        public string? WalletNumber { get; set; }
    }
}

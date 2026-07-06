namespace Finansal.PortfoyProject.Models
{
    public class Crypto
    {
        public int Id { get; set; }
        public string CryptoCode { get; set; }
        public string CryptoName { get; set; }
        public decimal Amount { get; set; } 
        public decimal AvgPrice { get; set; }
        public string? WalletAddress { get; set; } 
    }
}

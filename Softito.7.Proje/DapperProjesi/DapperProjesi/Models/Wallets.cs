namespace DapperProjesi.Models
{
    public class Wallets
    {
        public int Id { get; set; }
        public int UserId { get; set; } 
        public string WalletNumber { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public string Currency { get; set; } = "TRY"; 
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}

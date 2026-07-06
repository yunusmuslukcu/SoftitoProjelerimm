using Finansal.PortfoyProject.Data;
using Finansal.PortfoyProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Finansal.PortfoyProject.Pages.Cryptos
{
    public class CreateModel : PageModel
    {
        private readonly DbConnectionfactory _connectionFactory;

        public CreateModel(DbConnectionfactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        [BindProperty]
        public Crypto NewCrypto { get; set; }
        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            
            if (string.IsNullOrEmpty(NewCrypto.CryptoCode) || string.IsNullOrEmpty(NewCrypto.CryptoName))
            {
                return Page();
            }

            using (SqlConnection connection = _connectionFactory.CreateConnection())
            {
                
                string query = "INSERT INTO Cryptos (CryptoCode, CryptoName, Amount, AvgPrice, WalletAddress) " +
                               "VALUES (@CryptoCode, @CryptoName, @Amount, @AvgPrice, @WalletAddress)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    
                    command.Parameters.AddWithValue("@CryptoCode", NewCrypto.CryptoCode.ToUpper()); 
                    command.Parameters.AddWithValue("@CryptoName", NewCrypto.CryptoName);
                    command.Parameters.AddWithValue("@Amount", NewCrypto.Amount);
                    command.Parameters.AddWithValue("@AvgPrice", NewCrypto.AvgPrice);

                    
                    command.Parameters.AddWithValue("@WalletAddress", (object)NewCrypto.WalletAddress ?? DBNull.Value);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            
            return RedirectToPage("Index");
        }
    }
}

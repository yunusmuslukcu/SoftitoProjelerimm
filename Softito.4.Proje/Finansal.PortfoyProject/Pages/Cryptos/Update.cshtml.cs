using Finansal.PortfoyProject.Data;
using Finansal.PortfoyProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Finansal.PortfoyProject.Pages.Cryptos
{
    public class UpdateModel : PageModel
    {
        private readonly DbConnectionfactory _connectionFactory;

        public UpdateModel(DbConnectionfactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        [BindProperty]
        public Crypto CryptoToEdit { get; set; }
        public IActionResult OnGet(int id)
        {
            using (SqlConnection connection = _connectionFactory.CreateConnection())
            {
                string query = "SELECT Id, CryptoCode, CryptoName, Amount, AvgPrice, WalletAddress FROM Cryptos WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            CryptoToEdit = new Crypto
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                CryptoCode = reader["CryptoCode"].ToString(),
                                CryptoName = reader["CryptoName"].ToString(),
                                Amount = Convert.ToDecimal(reader["Amount"]),
                                AvgPrice = Convert.ToDecimal(reader["AvgPrice"]),
                                WalletAddress = reader["WalletAddress"] != DBNull.Value ? reader["WalletAddress"].ToString() : string.Empty
                            };
                        }
                    }
                }
            }


            if (CryptoToEdit == null)
            {
                return RedirectToPage("Index");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(CryptoToEdit.CryptoCode) || string.IsNullOrEmpty(CryptoToEdit.CryptoName))
            {
                return Page();
            }

            using (SqlConnection connection = _connectionFactory.CreateConnection())
            {
                
                string query = "UPDATE Cryptos SET CryptoCode = @CryptoCode, CryptoName = @CryptoName, " +
                               "Amount = @Amount, AvgPrice = @AvgPrice, WalletAddress = @WalletAddress WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    
                    command.Parameters.AddWithValue("@Id", CryptoToEdit.Id);
                    command.Parameters.AddWithValue("@CryptoCode", CryptoToEdit.CryptoCode.ToUpper());
                    command.Parameters.AddWithValue("@CryptoName", CryptoToEdit.CryptoName);
                    command.Parameters.AddWithValue("@Amount", CryptoToEdit.Amount);
                    command.Parameters.AddWithValue("@AvgPrice", CryptoToEdit.AvgPrice);

                    
                    command.Parameters.AddWithValue("@WalletAddress", (object)CryptoToEdit.WalletAddress ?? DBNull.Value);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            
            return RedirectToPage("Index");
        }
    }
}

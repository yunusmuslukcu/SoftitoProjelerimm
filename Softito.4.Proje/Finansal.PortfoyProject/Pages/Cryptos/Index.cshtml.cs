using Finansal.PortfoyProject.Data;
using Finansal.PortfoyProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Finansal.PortfoyProject.Pages.Cryptos
{
    public class IndexModel : PageModel
    {
        private readonly DbConnectionfactory _connectionFactory;


        public IndexModel(DbConnectionfactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public List<Crypto> CryptoList { get; set; } = new List<Crypto>();
        public void OnGet()
        {
            using (SqlConnection connection = _connectionFactory.CreateConnection())
            {
                
                string query = "SELECT Id, CryptoCode, CryptoName, Amount, AvgPrice, WalletAddress FROM Cryptos";

                
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    
                    connection.Open();

                    
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            
                            Crypto crypto = new Crypto
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                CryptoCode = reader["CryptoCode"].ToString(),
                                CryptoName = reader["CryptoName"].ToString(),
                                Amount = Convert.ToDecimal(reader["Amount"]), 
                                AvgPrice = Convert.ToDecimal(reader["AvgPrice"]),
                                WalletAddress = reader["WalletAddress"] != DBNull.Value ? reader["WalletAddress"].ToString() : string.Empty
                            };

                            // Listeye ekle
                            CryptoList.Add(crypto);
                        }
                    }
                }
            }
        }
    }
}

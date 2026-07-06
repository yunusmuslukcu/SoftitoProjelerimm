using Finansal.PortfoyProject.Data;
using Finansal.PortfoyProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Finansal.PortfoyProject.Pages.Cryptos
{
    public class DeleteModel : PageModel
    {

        private readonly DbConnectionfactory _connectionFactory;

        public DeleteModel(DbConnectionfactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        [BindProperty]
        public Crypto CryptoToDelete { get; set; }
        public IActionResult OnGet(int id)
        {
            using (SqlConnection connection = _connectionFactory.CreateConnection())
            {
                string query = "SELECT Id, CryptoCode, CryptoName FROM Cryptos WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            CryptoToDelete = new Crypto
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                CryptoCode = reader["CryptoCode"].ToString(),
                                CryptoName = reader["CryptoName"].ToString()
                            };
                        }
                    }
                }
            }


            if (CryptoToDelete == null)
            {
                return RedirectToPage("Index");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            using (SqlConnection connection = _connectionFactory.CreateConnection())
            {
                string query = "DELETE FROM Cryptos WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    
                    command.Parameters.AddWithValue("@Id", CryptoToDelete.Id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            
            return RedirectToPage("Index");
        }
    }
}

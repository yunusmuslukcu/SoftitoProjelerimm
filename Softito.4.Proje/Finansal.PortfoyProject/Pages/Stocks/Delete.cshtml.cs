using Finansal.PortfoyProject.Data;
using Finansal.PortfoyProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Finansal.PortfoyProject.Pages.Stocks
{
    public class DeleteModel : PageModel
    {

        private readonly DbConnectionfactory _connectionFactory;

        public DeleteModel(DbConnectionfactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        [BindProperty]
        public Stock StockToDelete { get; set; }

        public IActionResult OnGet(int id)
        {
            using (SqlConnection connection = _connectionFactory.CreateConnection())
            {
                string query = "SELECT Id, StockCode, StockName FROM Stocks WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            StockToDelete = new Stock
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                StockCode = reader["StockCode"].ToString(),
                                StockName = reader["StockName"].ToString()
                            };
                        }
                    }
                }
            }


            if (StockToDelete == null)
            {
                return RedirectToPage("Index");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            using (SqlConnection connection = _connectionFactory.CreateConnection())
            {
                string query = "DELETE FROM Stocks WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                
                    command.Parameters.AddWithValue("@Id", StockToDelete.Id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            
            return RedirectToPage("Index");
        }
    }
}



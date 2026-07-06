using Finansal.PortfoyProject.Data;
using Finansal.PortfoyProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Finansal.PortfoyProject.Pages.Stocks
{
    public class UpdateModel : PageModel
    {

        private readonly DbConnectionfactory _connectionFactory;

        public UpdateModel(DbConnectionfactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }


        [BindProperty]
        public Stock StockToEdit { get; set; }
        public IActionResult OnGet(int id)
        {
            using (SqlConnection connection = _connectionFactory.CreateConnection())
            {
                string query = "SELECT Id, StockCode, StockName, Quantity, AvgPrice, PurchaseDate FROM Stocks WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            StockToEdit = new Stock
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                StockCode = reader["StockCode"].ToString(),
                                StockName = reader["StockName"].ToString(),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                AvgPrice = Convert.ToDecimal(reader["AvgPrice"]),
                                PurchaseDate = Convert.ToDateTime(reader["PurchaseDate"])
                            };
                        }
                    }
                }
            }


            if (StockToEdit == null)
            {
                return RedirectToPage("Index");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(StockToEdit.StockCode) || string.IsNullOrEmpty(StockToEdit.StockName))
            {
                return Page();
            }

            using (SqlConnection connection = _connectionFactory.CreateConnection())
            {
                
                string query = "UPDATE Stocks SET StockCode = @StockCode, StockName = @StockName, " +
                               "Quantity = @Quantity, AvgPrice = @AvgPrice WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    
                    command.Parameters.AddWithValue("@Id", StockToEdit.Id);
                    command.Parameters.AddWithValue("@StockCode", StockToEdit.StockCode.ToUpper());
                    command.Parameters.AddWithValue("@StockName", StockToEdit.StockName);
                    command.Parameters.AddWithValue("@Quantity", StockToEdit.Quantity);
                    command.Parameters.AddWithValue("@AvgPrice", StockToEdit.AvgPrice);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            
            return RedirectToPage("Index");
        }
    }
}

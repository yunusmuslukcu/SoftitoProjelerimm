using Finansal.PortfoyProject.Data;
using Finansal.PortfoyProject.Models;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Finansal.PortfoyProject.Pages.Stocks
{
    public class CreateModel : PageModel
    {
        private readonly DbConnectionfactory _connectionFactory;

        public CreateModel(DbConnectionfactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        [BindProperty]
        public Stock NewStock { get; set; }

        public void OnGet()  //Sayfa ilk açıldığında formu boş göstermek için!
        {
        }

        public IActionResult OnPost() //kaydet butonunda form gönderilme için!
        {
            
            if (string.IsNullOrEmpty(NewStock.StockCode) || string.IsNullOrEmpty(NewStock.StockName))
            {
                return Page();
            }

            
            using (SqlConnection connection = _connectionFactory.CreateConnection())
            {
               
                string query = "INSERT INTO Stocks (StockCode, StockName, Quantity, AvgPrice, PurchaseDate) " +
                               "VALUES (@StockCode, @StockName, @Quantity, @AvgPrice, GETDATE())";

                
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    
                    command.Parameters.AddWithValue("@StockCode", NewStock.StockCode.ToUpper()); 
                    command.Parameters.AddWithValue("@StockName", NewStock.StockName);
                    command.Parameters.AddWithValue("@Quantity", NewStock.Quantity);
                    command.Parameters.AddWithValue("@AvgPrice", NewStock.AvgPrice);

                    
                    connection.Open();
                    command.ExecuteNonQuery(); 
                }
            }

            // İşlem tamamlandıktan sonra yönlendirme!
            return RedirectToPage("Index");
        }
    }
}

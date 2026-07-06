using Finansal.PortfoyProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Finansal.PortfoyProject.Pages
{
    public class IndexModel : PageModel
    {
        private readonly DbConnectionfactory _connectionFactory;

        public IndexModel(DbConnectionfactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public int TotalStocksCount { get; set; }
        public int TotalCryptosCount { get; set; }
        public decimal TotalStocksValue { get; set; }
        public decimal TotalCryptosValue { get; set; }

        public void OnGet()
        {
            using (SqlConnection connection = _connectionFactory.CreateConnection())
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT COUNT(Id) as TotalCount, ISNULL(SUM(Quantity * AvgPrice), 0) as TotalVal FROM Stocks", connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    reader.Read();
                    TotalStocksCount = Convert.ToInt32(reader["TotalCount"]);
                    TotalStocksValue = Convert.ToDecimal(reader["TotalVal"]);
                }

                using (SqlCommand command = new SqlCommand("SELECT COUNT(Id) as TotalCount, ISNULL(SUM(Amount * AvgPrice), 0) as TotalVal FROM Cryptos", connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    reader.Read();
                    TotalCryptosCount = Convert.ToInt32(reader["TotalCount"]);
                    TotalCryptosValue = Convert.ToDecimal(reader["TotalVal"]);
                }
            }
        }
    }
}

using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BicyleApi.Models
{
    public class Context
    {
        public static string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=VeloRentDB;Integrated Security=true;TrustServerCertificate=True;";


        public static void ExecuteReturn(string procadi, DynamicParameters param = null)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                db.Execute(procadi, param, commandType: CommandType.StoredProcedure);
            }
        }

        
        public static IEnumerable<T> Listeleme<T>(string procadi, DynamicParameters param = null)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                return db.Query<T>(procadi, param, commandType: CommandType.StoredProcedure);
            }
        }

        public static IEnumerable<T> QueryRaw<T>(string query, DynamicParameters param = null)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                return db.Query<T>(query, param, commandType: CommandType.Text);
            }
        }
    }
}

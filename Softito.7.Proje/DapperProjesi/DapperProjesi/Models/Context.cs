using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperProjesi.Models
{
    public class Context
    {
        
        
        public static string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=Finance;Integrated Security=true;TrustServerCertificate=True;";

        // Ekleme, Güncelleme, Silme işlemleri
        public static void ExecuteReturn(string procadi, DynamicParameters? param = null)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                db.Execute(procadi, param, commandType: CommandType.StoredProcedure);
            }
        }

        // Listeleme 
        public static IEnumerable<T> Listeleme<T>(string procadi, DynamicParameters? param = null)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                return db.Query<T>(procadi, param, commandType: CommandType.StoredProcedure);
            }
        }
    }
}

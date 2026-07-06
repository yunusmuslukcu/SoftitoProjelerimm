using Microsoft.Data.SqlClient;

namespace Finansal.PortfoyProject.Data
{
    public class DbConnectionfactory
    {
        private readonly IConfiguration _configuration;

        public DbConnectionfactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SqlConnection CreateConnection()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            return new SqlConnection(connectionString);
        }

        public void InitializeDatabase()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
            string targetDb = builder.InitialCatalog;
            builder.InitialCatalog = "master";
            string masterConnectionString = builder.ConnectionString;

            using (SqlConnection masterConnection = new SqlConnection(masterConnectionString))
            {
                masterConnection.Open();
                string checkDbQuery = $"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'{targetDb}') CREATE DATABASE [{targetDb}];";
                using (SqlCommand cmd = new SqlCommand(checkDbQuery, masterConnection))
                {
                    cmd.ExecuteNonQuery();
                }
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                
                string createTablesQuery = @"
                    IF OBJECT_ID('Stocks', 'U') IS NULL
                    BEGIN
                        CREATE TABLE Stocks (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            StockCode NVARCHAR(50) NOT NULL,
                            StockName NVARCHAR(100) NOT NULL,
                            Quantity INT NOT NULL,
                            AvgPrice DECIMAL(18,2) NOT NULL,
                            PurchaseDate DATETIME NOT NULL
                        );
                    END;

                    IF OBJECT_ID('Cryptos', 'U') IS NULL
                    BEGIN
                        CREATE TABLE Cryptos (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            CryptoCode NVARCHAR(50) NOT NULL,
                            CryptoName NVARCHAR(100) NOT NULL,
                            Amount DECIMAL(18,8) NOT NULL,
                            AvgPrice DECIMAL(18,2) NOT NULL,
                            WalletAddress NVARCHAR(200) NULL
                        );
                    END;";

                using (SqlCommand cmd = new SqlCommand(createTablesQuery, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

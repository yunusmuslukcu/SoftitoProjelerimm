namespace DapperProjesi.Models
{
 
    
        public class Products
        {
            public int Id { get; set; }
            public string ProductName { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public int Stock { get; set; }
            public bool IsActive { get; set; } = true;
        }
    
}


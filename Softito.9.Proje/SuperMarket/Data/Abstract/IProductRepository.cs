using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Abstract
{
    public interface IProductRepository:IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsWithDetailsAsync();
    }
}

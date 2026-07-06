using Data.Abstract;
using Microsoft.EntityFrameworkCore;
using Model;
using SupermarketManagement.Data.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Product>> GetProductsWithDetailsAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .ToListAsync();
        }
    }
}

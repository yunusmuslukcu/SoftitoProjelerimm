using IoTSystemApi.Data;
using IoTSystemApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IoTProductionSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public ProductsController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("GetProducts")]
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await dbContext.Products.ToListAsync();
        }

        [HttpGet]
        [Route("GetProductById/{id}")]
        public async Task<Product> GetProductById(int id)
        {
            // Hocanın FindAsync kullanım tarzına birebir uygun
            return await dbContext.FindAsync<Product>(id);
        }

        [HttpPost]
        [Route("AddProduct")]
        public async Task<Product> AddProduct(Product product)
        {
            dbContext.Add(product);
            await dbContext.SaveChangesAsync();
            return product;
        }

        [HttpPut]
        [Route("UpdateProduct/{id}")]
        public async Task<Product> UpdateProduct(Product product)
        {
            dbContext.Update(product);
            await dbContext.SaveChangesAsync();
            return product;
        }

        [HttpDelete]
        [Route("DeleteProduct/{id}")]
        public bool DeleteProduct(int id)
        {
            var islem = false;
            var result = dbContext.Products.Find(id);
            if (result != null)
            {
                islem = true;
                dbContext.Remove(result);
                dbContext.SaveChanges();
            }
            else
            {
                return islem;
            }
            return islem;
        }
    }
}
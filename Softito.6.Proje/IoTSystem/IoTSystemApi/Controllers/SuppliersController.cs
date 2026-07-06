using IoTSystemApi.Data;
using IoTSystemApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IoTProductionSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public SuppliersController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("GetSuppliers")]
        public async Task<IEnumerable<Supplier>> GetSuppliers()
        {
            return await dbContext.Suppliers.ToListAsync();
        }

        [HttpGet]
        [Route("GetSupplierById/{id}")]
        public async Task<Supplier> GetSupplierById(int id)
        {
            return await dbContext.FindAsync<Supplier>(id);
        }

        [HttpPost]
        [Route("AddSupplier")]
        public async Task<Supplier> AddSupplier(Supplier supplier)
        {
            dbContext.Add(supplier);
            await dbContext.SaveChangesAsync();
            return supplier;
        }

        [HttpPut]
        [Route("UpdateSupplier/{id}")]
        public async Task<Supplier> UpdateSupplier(Supplier supplier)
        {
            dbContext.Update(supplier);
            await dbContext.SaveChangesAsync();
            return supplier;
        }

        [HttpDelete]
        [Route("DeleteSupplier/{id}")]
        public bool DeleteSupplier(int id)
        {
            var islem = false;
            var result = dbContext.Suppliers.Find(id);
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
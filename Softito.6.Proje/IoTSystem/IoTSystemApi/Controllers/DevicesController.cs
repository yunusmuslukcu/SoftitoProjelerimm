using IoTSystemApi.Data;
using IoTSystemApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IoTProductionSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public DevicesController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("GetDevices")]
        public async Task<IEnumerable<Device>> GetDevices()
        {
            return await dbContext.Devices.ToListAsync();
        }

        [HttpGet]
        [Route("GetDeviceById/{id}")]
        public async Task<Device> GetDeviceById(int id)
        {
            return await dbContext.FindAsync<Device>(id);
        }

        [HttpPost]
        [Route("AddDevice")]
        public async Task<Device> AddDevice(Device device)
        {
            dbContext.Add(device);
            await dbContext.SaveChangesAsync();
            return device;
        }

        [HttpPut]
        [Route("UpdateDevice/{id}")]
        public async Task<Device> UpdateDevice(Device device)
        {
            dbContext.Update(device);
            await dbContext.SaveChangesAsync();
            return device;
        }

        [HttpDelete]
        [Route("DeleteDevice/{id}")]
        public bool DeleteDevice(int id)
        {
            var islem = false;
            var result = dbContext.Devices.Find(id);
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
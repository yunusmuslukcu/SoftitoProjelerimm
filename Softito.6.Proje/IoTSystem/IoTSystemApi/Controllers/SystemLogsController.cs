using IoTSystemApi.Data;
using IoTSystemApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IoTProductionSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemLogsController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public SystemLogsController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("GetSystemLogs")]
        public async Task<IEnumerable<SystemLog>> GetSystemLogs()
        {
            return await dbContext.SystemLogs.ToListAsync();
        }

        [HttpGet]
        [Route("GetSystemLogById/{id}")]
        public async Task<SystemLog> GetSystemLogById(int id)
        {
            return await dbContext.FindAsync<SystemLog>(id);
        }

        [HttpPost]
        [Route("AddSystemLog")]
        public async Task<SystemLog> AddSystemLog(SystemLog systemLog)
        {
            dbContext.Add(systemLog);
            await dbContext.SaveChangesAsync();
            return systemLog;
        }

        [HttpPut]
        [Route("UpdateSystemLog/{id}")]
        public async Task<SystemLog> UpdateSystemLog(SystemLog systemLog)
        {
            dbContext.Update(systemLog);
            await dbContext.SaveChangesAsync();
            return systemLog;
        }

        [HttpDelete]
        [Route("DeleteSystemLog/{id}")]
        public bool DeleteSystemLog(int id)
        {
            var islem = false;
            var result = dbContext.SystemLogs.Find(id);
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
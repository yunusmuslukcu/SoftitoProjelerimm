using IoTSystemApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IoTProductionSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public ReportsController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("GetReportData")]
        public async Task<IActionResult> GetReportData()
        {
            // 1. Product & Supplier (INNER JOIN, ORDER BY unit price desc)
            var productSupplierReport = await dbContext.Products
                .Join(dbContext.Suppliers,
                    p => p.SupplierId,
                    s => s.Id,
                    (p, s) => new
                    {
                        ProductId = p.Id,
                        ProductName = p.ProductName,
                        ProductCode = p.ProductCode,
                        UnitPrice = p.UnitPrice,
                        StockQuantity = p.StockQuantity,
                        SupplierName = s.SupplierName
                    })
                .OrderByDescending(p => p.UnitPrice)
                .ToListAsync();

            // 2. Device Logs Count (INNER JOIN, GROUP BY, COUNT, ORDER BY count desc)
            var deviceLogReport = await dbContext.SystemLogs
                .Join(dbContext.Devices,
                    l => l.DeviceId,
                    d => d.Id,
                    (l, d) => new { l, d })
                .GroupBy(x => new { x.d.DeviceName, x.d.Location })
                .Select(g => new
                {
                    DeviceName = g.Key.DeviceName,
                    Location = g.Key.Location,
                    LogCount = g.Count()
                })
                .OrderByDescending(x => x.LogCount)
                .ToListAsync();

            // 3. Log Levels Count (GROUP BY, COUNT, ORDER BY count desc)
            var logLevelReport = await dbContext.SystemLogs
                .GroupBy(l => l.LogLevel)
                .Select(g => new
                {
                    LogLevel = g.Key,
                    LogCount = g.Count()
                })
                .OrderByDescending(x => x.LogCount)
                .ToListAsync();

            // 4. Product Stock Summary by Supplier (INNER JOIN, GROUP BY, COUNT, SUM, ORDER BY stock desc)
            var productStockReport = await dbContext.Products
                .Join(dbContext.Suppliers,
                    p => p.SupplierId,
                    s => s.Id,
                    (p, s) => new { p, s })
                .GroupBy(x => x.s.SupplierName)
                .Select(g => new
                {
                    SupplierName = g.Key,
                    ProductCount = g.Count(),
                    TotalStock = g.Sum(x => x.p.StockQuantity)
                })
                .OrderByDescending(x => x.TotalStock)
                .ToListAsync();

            return Ok(new
            {
                ProductSupplierReport = productSupplierReport,
                DeviceLogReport = deviceLogReport,
                LogLevelReport = logLevelReport,
                ProductStockReport = productStockReport
            });
        }
    }
}

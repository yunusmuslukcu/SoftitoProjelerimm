using IoTSystemApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IoTSystemApi.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SystemLog> SystemLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Identity tablolarının konfigürasyonu için bu base metodun çağrılması gerekli.
            base.OnModelCreating(builder);

            // Seed Suppliers
            builder.Entity<Supplier>().HasData(
                new Supplier { Id = 1, SupplierName = "Siemens AG", ContactName = "Hans Müller", Phone = "+49 89 63600", Email = "contact@siemens.com", Address = "Munich, Germany" },
                new Supplier { Id = 2, SupplierName = "Robert Bosch GmbH", ContactName = "Fritz Weber", Phone = "+49 711 40040", Email = "info@bosch.com", Address = "Stuttgart, Germany" },
                new Supplier { Id = 3, SupplierName = "Schneider Electric", ContactName = "Pierre Dubois", Phone = "+33 1 4129 7000", Email = "support@schneider.com", Address = "Paris, France" },
                new Supplier { Id = 4, SupplierName = "Philips Hue", ContactName = "Jan de Jong", Phone = "+31 20 500 8000", Email = "jan@philips.com", Address = "Amsterdam, Netherlands" }
            );

            // Seed Devices
            builder.Entity<Device>().HasData(
                new Device { Id = 1, DeviceName = "Assembly Line Gateway", IpAddress = "192.168.1.10", MacAddress = "00:1A:2B:3C:4D:5E", IsActive = true, Location = "Assembly Line A" },
                new Device { Id = 2, DeviceName = "Warehouse Temp Sensor", IpAddress = "192.168.1.11", MacAddress = "00:1A:2B:3C:4D:5F", IsActive = true, Location = "Warehouse 1" },
                new Device { Id = 3, DeviceName = "Boiler Room Controller", IpAddress = "192.168.1.12", MacAddress = "00:1A:2B:3C:4D:60", IsActive = false, Location = "Boiler Room" },
                new Device { Id = 4, DeviceName = "Packing Node Relay", IpAddress = "192.168.1.13", MacAddress = "00:1A:2B:3C:4D:61", IsActive = true, Location = "Packing Zone" }
            );

            // Seed Products
            builder.Entity<Product>().HasData(
                new Product { Id = 1, ProductName = "IoT Gateway Controller", ProductCode = "GW-100", UnitPrice = 1500, StockQuantity = 45, CreatedDate = new DateTime(2026, 1, 15), SupplierId = 1 },
                new Product { Id = 2, ProductName = "Smart Temp/Humidity Sensor", ProductCode = "TS-202", UnitPrice = 350, StockQuantity = 120, CreatedDate = new DateTime(2026, 2, 20), SupplierId = 1 },
                new Product { Id = 3, ProductName = "Industrial Pressure Gauge", ProductCode = "PG-505", UnitPrice = 850, StockQuantity = 30, CreatedDate = new DateTime(2026, 3, 10), SupplierId = 2 },
                new Product { Id = 4, ProductName = "Wireless Relay Switch", ProductCode = "RS-303", UnitPrice = 220, StockQuantity = 200, CreatedDate = new DateTime(2026, 4, 5), SupplierId = 3 },
                new Product { Id = 5, ProductName = "LED Smart Controller Module", ProductCode = "LC-404", UnitPrice = 410, StockQuantity = 85, CreatedDate = new DateTime(2026, 5, 12), SupplierId = 4 }
            );

            // Seed SystemLogs
            builder.Entity<SystemLog>().HasData(
                new SystemLog { Id = 1, LogCode = "LOG-001", LogLevel = "Information", Description = "Device rebooted successfully after firmware update", FixSuggestion = "None required", LogDate = new DateTime(2026, 6, 22, 10, 30, 0), DeviceId = 1 },
                new SystemLog { Id = 2, LogCode = "LOG-002", LogLevel = "Warning", Description = "Warehouse temperature reached warning threshold of 45°C", FixSuggestion = "Check cooling system or ventilation", LogDate = new DateTime(2026, 6, 23, 14, 15, 0), DeviceId = 2 },
                new SystemLog { Id = 3, LogCode = "LOG-003", LogLevel = "Critical", Description = "Connection timed out, Boiler Room Controller unreachable", FixSuggestion = "Check network cabling and power supply status", LogDate = new DateTime(2026, 6, 24, 8, 0, 0), DeviceId = 3 },
                new SystemLog { Id = 4, LogCode = "LOG-004", LogLevel = "Error", Description = "Gateway failed to sync data with central cloud API", FixSuggestion = "Verify external internet connection and gateway DNS settings", LogDate = new DateTime(2026, 6, 24, 12, 45, 0), DeviceId = 1 }
            );
        }

    }
}


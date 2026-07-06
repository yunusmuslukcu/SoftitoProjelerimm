using System;
using System.Collections.Generic;

namespace IoTSystemMvc.Models
{
    public class ReportsViewModel
    {
        public List<ProductSupplierReportItem> ProductSupplierReport { get; set; } = new();
        public List<DeviceLogReportItem> DeviceLogReport { get; set; } = new();
        public List<LogLevelReportItem> LogLevelReport { get; set; } = new();
        public List<ProductStockReportItem> ProductStockReport { get; set; } = new();
    }

    public class ProductSupplierReportItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductCode { get; set; } = string.Empty;
        public int UnitPrice { get; set; }
        public int StockQuantity { get; set; }
        public string SupplierName { get; set; } = string.Empty;
    }

    public class DeviceLogReportItem
    {
        public string DeviceName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int LogCount { get; set; }
    }

    public class LogLevelReportItem
    {
        public string LogLevel { get; set; } = string.Empty;
        public int LogCount { get; set; }
    }

    public class ProductStockReportItem
    {
        public string SupplierName { get; set; } = string.Empty;
        public int ProductCount { get; set; }
        public int TotalStock { get; set; }
    }
}

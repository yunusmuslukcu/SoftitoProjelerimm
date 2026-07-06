using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IoTSystemApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationsAndSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeviceId",
                table: "SystemLogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "Id", "DeviceName", "IpAddress", "IsActive", "Location", "MacAddress" },
                values: new object[,]
                {
                    { 1, "Assembly Line Gateway", "192.168.1.10", true, "Assembly Line A", "00:1A:2B:3C:4D:5E" },
                    { 2, "Warehouse Temp Sensor", "192.168.1.11", true, "Warehouse 1", "00:1A:2B:3C:4D:5F" },
                    { 3, "Boiler Room Controller", "192.168.1.12", false, "Boiler Room", "00:1A:2B:3C:4D:60" },
                    { 4, "Packing Node Relay", "192.168.1.13", true, "Packing Zone", "00:1A:2B:3C:4D:61" }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "Id", "Address", "ContactName", "Email", "Phone", "SupplierName" },
                values: new object[,]
                {
                    { 1, "Munich, Germany", "Hans Müller", "contact@siemens.com", "+49 89 63600", "Siemens AG" },
                    { 2, "Stuttgart, Germany", "Fritz Weber", "info@bosch.com", "+49 711 40040", "Robert Bosch GmbH" },
                    { 3, "Paris, France", "Pierre Dubois", "support@schneider.com", "+33 1 4129 7000", "Schneider Electric" },
                    { 4, "Amsterdam, Netherlands", "Jan de Jong", "jan@philips.com", "+31 20 500 8000", "Philips Hue" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CreatedDate", "ProductCode", "ProductName", "StockQuantity", "SupplierId", "UnitPrice" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "GW-100", "IoT Gateway Controller", 45, 1, 1500 },
                    { 2, new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "TS-202", "Smart Temp/Humidity Sensor", 120, 1, 350 },
                    { 3, new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "PG-505", "Industrial Pressure Gauge", 30, 2, 850 },
                    { 4, new DateTime(2026, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "RS-303", "Wireless Relay Switch", 200, 3, 220 },
                    { 5, new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "LC-404", "LED Smart Controller Module", 85, 4, 410 }
                });

            migrationBuilder.InsertData(
                table: "SystemLogs",
                columns: new[] { "Id", "Description", "DeviceId", "FixSuggestion", "LogCode", "LogDate", "LogLevel" },
                values: new object[,]
                {
                    { 1, "Device rebooted successfully after firmware update", 1, "None required", "LOG-001", new DateTime(2026, 6, 22, 10, 30, 0, 0, DateTimeKind.Unspecified), "Information" },
                    { 2, "Warehouse temperature reached warning threshold of 45°C", 2, "Check cooling system or ventilation", "LOG-002", new DateTime(2026, 6, 23, 14, 15, 0, 0, DateTimeKind.Unspecified), "Warning" },
                    { 3, "Connection timed out, Boiler Room Controller unreachable", 3, "Check network cabling and power supply status", "LOG-003", new DateTime(2026, 6, 24, 8, 0, 0, 0, DateTimeKind.Unspecified), "Critical" },
                    { 4, "Gateway failed to sync data with central cloud API", 1, "Verify external internet connection and gateway DNS settings", "LOG-004", new DateTime(2026, 6, 24, 12, 45, 0, 0, DateTimeKind.Unspecified), "Error" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SystemLogs_DeviceId",
                table: "SystemLogs",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SupplierId",
                table: "Products",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Suppliers_SupplierId",
                table: "Products",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemLogs_Devices_DeviceId",
                table: "SystemLogs",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Suppliers_SupplierId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemLogs_Devices_DeviceId",
                table: "SystemLogs");

            migrationBuilder.DropIndex(
                name: "IX_SystemLogs_DeviceId",
                table: "SystemLogs");

            migrationBuilder.DropIndex(
                name: "IX_Products_SupplierId",
                table: "Products");

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "SystemLogs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SystemLogs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SystemLogs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "SystemLogs",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "SystemLogs");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "Products");
        }
    }
}

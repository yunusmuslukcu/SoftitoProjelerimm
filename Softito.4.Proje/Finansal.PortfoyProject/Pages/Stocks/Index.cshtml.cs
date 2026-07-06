using Finansal.PortfoyProject.Data;
using Finansal.PortfoyProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.IO;

namespace Finansal.PortfoyProject.Pages.Stocks
{
    public class IndexModel : PageModel
    {
        private readonly DbConnectionfactory _connectionFactory;

        public IndexModel(DbConnectionfactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public List<Stock> StockList { get; set; } = new List<Stock>();

        public void OnGet()
        {
            using (SqlConnection connection = _connectionFactory.CreateConnection())
            {
                string query = "SELECT Id, StockCode, StockName, Quantity, AvgPrice, PurchaseDate FROM Stocks";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Stock stock = new Stock
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                StockCode = reader["StockCode"].ToString(),
                                StockName = reader["StockName"].ToString(),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                AvgPrice = Convert.ToDecimal(reader["AvgPrice"]),
                                PurchaseDate = Convert.ToDateTime(reader["PurchaseDate"])
                            };

                            StockList.Add(stock);
                        }
                    }
                }
            } 
        }

        public IActionResult OnGetDownloadPdf()
        {
            OnGet();

            PdfDocument document = new PdfDocument();
            document.Info.Title = "Stok Portföy Raporu";

            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont titleFont = new XFont("Arial", 18, XFontStyleEx.Bold);
            XFont headerFont = new XFont("Arial", 10, XFontStyleEx.Bold);
            XFont textFont = new XFont("Arial", 10, XFontStyleEx.Regular);

            gfx.DrawString("STOK PORTFÖY RAPORU", titleFont, XBrushes.DarkSlateGray, new XPoint(40, 50));
            gfx.DrawString("Tarih: " + DateTime.Now.ToShortDateString(), textFont, XBrushes.Gray, new XPoint(40, 75));

            int startX = 40;
            int startY = 110;
            int rowHeight = 22;

            gfx.DrawRectangle(XPens.LightGray, XBrushes.GhostWhite, startX, startY, 515, rowHeight);
            
            gfx.DrawString("Hisse Kodu", headerFont, XBrushes.Black, new XPoint(startX + 10, startY + 15));
            gfx.DrawString("Hisse Adı", headerFont, XBrushes.Black, new XPoint(startX + 100, startY + 15));
            gfx.DrawString("Adet", headerFont, XBrushes.Black, new XPoint(startX + 250, startY + 15));
            gfx.DrawString("Ort. Fiyat", headerFont, XBrushes.Black, new XPoint(startX + 320, startY + 15));
            gfx.DrawString("Toplam Maliyet", headerFont, XBrushes.Black, new XPoint(startX + 400, startY + 15));

            int currentY = startY + rowHeight;

            foreach (var stock in StockList)
            {
                gfx.DrawRectangle(XPens.LightGray, startX, currentY, 515, rowHeight);

                gfx.DrawString(stock.StockCode, textFont, XBrushes.Black, new XPoint(startX + 10, currentY + 15));
                gfx.DrawString(stock.StockName, textFont, XBrushes.Black, new XPoint(startX + 100, currentY + 15));
                gfx.DrawString(stock.Quantity.ToString(), textFont, XBrushes.Black, new XPoint(startX + 250, currentY + 15));
                gfx.DrawString(stock.AvgPrice.ToString("C2"), textFont, XBrushes.Black, new XPoint(startX + 320, currentY + 15));
                gfx.DrawString((stock.Quantity * stock.AvgPrice).ToString("C2"), textFont, XBrushes.Black, new XPoint(startX + 400, currentY + 15));

                currentY += rowHeight;
            }

            using (MemoryStream stream = new MemoryStream())
            {
                document.Save(stream);
                return File(stream.ToArray(), "application/pdf", "StokPortfoy.pdf");
            }
        }

        public IActionResult OnGetDownloadWord()
        {
            OnGet();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<html>");
            sb.Append("<head><meta charset='utf-8'><style>table { border-collapse: collapse; width: 100%; } th, td { border: 1px solid #ddd; padding: 8px; text-align: left; } th { background-color: #f2f2f2; }</style></head>");
            sb.Append("<body>");
            sb.Append("<h2>Stok Portföy Raporu</h2>");
            sb.Append("<p>Tarih: " + DateTime.Now.ToShortDateString() + "</p>");
            sb.Append("<table>");
            sb.Append("<tr><th>Hisse Kodu</th><th>Hisse Adı</th><th>Adet</th><th>Ortalama Fiyat</th><th>Toplam Maliyet</th><th>Tarih</th></tr>");
            
            foreach (var stock in StockList)
            {
                sb.Append("<tr>");
                sb.Append("<td>" + stock.StockCode + "</td>");
                sb.Append("<td>" + stock.StockName + "</td>");
                sb.Append("<td>" + stock.Quantity + "</td>");
                sb.Append("<td>" + stock.AvgPrice.ToString("C2") + "</td>");
                sb.Append("<td>" + (stock.Quantity * stock.AvgPrice).ToString("C2") + "</td>");
                sb.Append("<td>" + stock.PurchaseDate.ToShortDateString() + "</td>");
                sb.Append("</tr>");
            }
            
            sb.Append("</table>");
            sb.Append("</body>");
            sb.Append("</html>");

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
            return File(bytes, "application/msword", "StokPortfoy.doc");
        }
    }
}

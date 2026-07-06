using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Marketing.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Grafikler için fiyat verileri (örnek olarak ürün fiyatları)
            var productPrices = _context.Products
                .Select(p => new { p.Name, p.Price })
                .OrderBy(p => p.Price)
                .ToList();

            ViewBag.ChartLabels = productPrices.Select(p => p.Name).ToList();
            ViewBag.ChartData = productPrices.Select(p => p.Price).ToList();

            // En uygun 4 ürün
            var topProducts = _context.Products
                .OrderBy(p => p.Price)
                .Take(4)
                .ToList();

            return View(topProducts);
        }
    }
}

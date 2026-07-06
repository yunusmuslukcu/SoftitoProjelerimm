using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Marketing.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ReportController : Controller
    {
        private readonly AppDbContext _context;

        public ReportController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // 1. INNER JOIN & ORDER BY: Ürünler ve Tedarikçiler birleşimi, fiyata göre sıralı
            var productsWithSuppliers = _context.Products
                .Include(p => p.Supplier)
                .OrderByDescending(p => p.Price)
                .Select(p => new ProductSupplierReport
                {
                    ProductName = p.Name,
                    Price = p.Price,
                    SupplierName = p.Supplier.CompanyName
                })
                .ToList();

            // 2. GROUP BY & COUNT: Kategori bazında ürün sayısı
            var productsByCategory = _context.Products
                .Include(p => p.Category)
                .GroupBy(p => p.Category.Name)
                .Select(g => new CategoryCountReport
                {
                    CategoryName = g.Key,
                    ProductCount = g.Count()
                })
                .ToList();

            // 3. LEFT JOIN benzeri: Tedarikçiler ve sağladıkları ürün sayıları
            // (Tüm tedarikçileri getir, hiç ürünü olmayanlar da 0 olarak gözüksün)
            var supplierProductCounts = _context.Suppliers
                .Select(s => new SupplierProductCountReport
                {
                    SupplierName = s.CompanyName,
                    ProductCount = _context.Products.Count(p => p.SupplierId == s.Id)
                })
                .OrderByDescending(x => x.ProductCount)
                .ToList();

            ViewBag.ProductsWithSuppliers = productsWithSuppliers;
            ViewBag.ProductsByCategory = productsByCategory;
            ViewBag.SupplierProductCounts = supplierProductCounts;

            return View();
        }
    }

    // Report DTOs
    public class ProductSupplierReport
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string SupplierName { get; set; }
    }

    public class CategoryCountReport
    {
        public string CategoryName { get; set; }
        public int ProductCount { get; set; }
    }

    public class SupplierProductCountReport
    {
        public string SupplierName { get; set; }
        public int ProductCount { get; set; }
    }
}

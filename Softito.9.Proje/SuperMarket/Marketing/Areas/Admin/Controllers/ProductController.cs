using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Data;
using Model;
using Microsoft.EntityFrameworkCore;

namespace Marketing.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            var products = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name");
            ViewBag.Suppliers = new SelectList(_context.Suppliers.ToList(), "Id", "CompanyName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile? imageFile)
        {
            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "products");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    product.ImageUrl = "/uploads/products/" + uniqueFileName;
                }

                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name");
                ViewBag.Suppliers = new SelectList(_context.Suppliers.ToList(), "Id", "CompanyName");
                ViewBag.Error = "Ürün eklenirken bir hata oluştu.";
                return View(product);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound();

            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name", product.CategoryId);
            ViewBag.Suppliers = new SelectList(_context.Suppliers.ToList(), "Id", "CompanyName", product.SupplierId);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product, IFormFile? imageFile)
        {
            try
            {
                var existing = _context.Products.Find(product.Id);
                if (existing == null) return NotFound();

                existing.Name = product.Name;
                existing.Barcode = product.Barcode;
                existing.Price = product.Price;
                existing.StockQuantity = product.StockQuantity;
                existing.CategoryId = product.CategoryId;
                existing.SupplierId = product.SupplierId;

                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "products");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    existing.ImageUrl = "/uploads/products/" + uniqueFileName;
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name");
                ViewBag.Suppliers = new SelectList(_context.Suppliers.ToList(), "Id", "CompanyName");
                ViewBag.Error = "Ürün güncellenirken bir hata oluştu.";
                return View(product);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}

using GymAppLite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 

namespace GymAppLite.Controllers
{
    public class ProgressLogsController : Controller
    {
        public readonly GymContext DbContext;

        public ProgressLogsController(GymContext dbcontext)
        {
            this.DbContext = dbcontext;
        }

        // 1. LİSTELEME EKRANI (INDEX)
        public IActionResult Index(string? search)
        {
            var query = DbContext.ProgressLogs
                .Include(p => p.Member)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Member.FirstName.Contains(search) || 
                                     p.Member.LastName.Contains(search));
            }

            var result = query.ToList();
            return View(result);
        }

        // 2. EKLEME SAYFASI (GET)
        [HttpGet]
        public IActionResult Create()
        {
           
            ViewBag.Members = DbContext.Members.ToList();

            return View();
        }

        // 3. EKLEME İŞLEMİ (POST)
        [HttpPost]
        public IActionResult Create(ProgressLog progressLog)
        {
            DbContext.ProgressLogs.Add(progressLog);
            DbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        // 4. GÜNCELLEME SAYFASI (GET)
        [HttpGet]
        public IActionResult Update(int id)
        {
            var result = DbContext.ProgressLogs.Find(id);

          
            ViewBag.Members = DbContext.Members.ToList();

            return View(result);
        }

        // 5. GÜNCELLEME İŞLEMİ (POST)
        [HttpPost]
        public IActionResult Update(ProgressLog progressLog)
        {
            DbContext.Update(progressLog);
            DbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        // 6. SİLME ONAY SAYFASI (GET)
        [HttpGet]
        public IActionResult Delete(int id)
        {
            
            var result = DbContext.ProgressLogs
                .Include(p => p.Member)
                .FirstOrDefault(p => p.Id == id);

            return View(result);
        }

        // 7. SİLME İŞLEMİ (POST)
        [HttpPost]
        public IActionResult Delete(ProgressLog progressLog)
        {
            DbContext.Remove(progressLog);
            DbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
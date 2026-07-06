using GymAppLite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 

namespace GymAppLite.Controllers
{
    public class MembersController : Controller
    {
        public readonly GymContext DbContext;

        public MembersController(GymContext dbcontext)
        {
            this.DbContext = dbcontext;
        }

        
        public IActionResult Index(string? search)
        {
            var query = DbContext.Members
                .Include(m => m.SubscriptionType)
                .Include(m => m.Trainer)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(m => m.FirstName.Contains(search) || 
                                     m.LastName.Contains(search) ||
                                     (m.SubscriptionType != null && m.SubscriptionType.TypeName.Contains(search)) ||
                                     (m.Trainer != null && m.Trainer.FullName.Contains(search)));
            }

            var result = query.ToList();
            return View(result);
        }

        // 2. EKLEME SAYFASI (GET)
        [HttpGet]
        public IActionResult Create()
        {
            
            ViewBag.SubscriptionTypes = DbContext.SubscriptionTypes.ToList();
            ViewBag.Trainers = DbContext.Trainers.ToList();

            return View();
        }

        // 3. EKLEME İŞLEMİ (POST)
        [HttpPost]
        public IActionResult Create(Member member)
        {
            DbContext.Members.Add(member);
            DbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        // 4. GÜNCELLEME SAYFASI (GET)
        [HttpGet]
        public IActionResult Update(int id)
        {
            var result = DbContext.Members.Find(id);

            
            ViewBag.SubscriptionTypes = DbContext.SubscriptionTypes.ToList();
            ViewBag.Trainers = DbContext.Trainers.ToList();

            return View(result);
        }

        // 5. GÜNCELLEME İŞLEMİ (POST)
        [HttpPost]
        public IActionResult Update(Member member)
        {
            DbContext.Update(member);
            DbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        // 6. SİLME ONAY SAYFASI (GET)
        [HttpGet]
        public IActionResult Delete(int id)
        {
            
            var result = DbContext.Members
                .Include(m => m.SubscriptionType)
                .Include(m => m.Trainer)
                .FirstOrDefault(m => m.Id == id);

            return View(result);
        }

        // 7. SİLME İŞLEMİ (POST)
        [HttpPost]
        public IActionResult Delete(Member member)
        {
            DbContext.Remove(member);
            DbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
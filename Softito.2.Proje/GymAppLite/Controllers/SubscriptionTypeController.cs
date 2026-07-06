using GymAppLite.Models;
using Microsoft.AspNetCore.Mvc;

namespace GymAppLite.Controllers
{
    public class SubscriptionTypeController : Controller
    {
        public readonly GymContext DbContext;

        public SubscriptionTypeController(GymContext dbcontext)
        {
            this.DbContext = dbcontext;
        }
        public IActionResult Index(string? search)
        {
            var query = DbContext.SubscriptionTypes.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.TypeName.Contains(search));
            }

            var result = query.ToList();
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(SubscriptionType subscriptionType)
        {
            DbContext.SubscriptionTypes.Add(subscriptionType);
            DbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]

        public IActionResult Update(int id)
        {
            var result = DbContext.SubscriptionTypes.Find(id);
            return View(result);
        }

        [HttpPost]

        public IActionResult Update(SubscriptionType subscriptionType)
        {
            DbContext.Update(subscriptionType);
            DbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var result = DbContext.SubscriptionTypes.Find(id);
            return View(result);
        }

        [HttpPost]

        public IActionResult Delete(SubscriptionType subscriptionType)
        {
            DbContext.Remove(subscriptionType);
            DbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

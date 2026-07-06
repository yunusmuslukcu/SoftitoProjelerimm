using GymAppLite.Models;
using Microsoft.AspNetCore.Mvc;

namespace GymAppLite.Controllers
{
    public class TrainersController : Controller
    {
        public readonly GymContext DbContext;

        public TrainersController(GymContext dbcontext)
        {
            this.DbContext = dbcontext;
        }
        public IActionResult Index(string? search)
        {
            var query = DbContext.Trainers.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(t => t.FullName.Contains(search) || 
                                     (t.Expertise != null && t.Expertise.Contains(search)));
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
        public IActionResult Create(Trainer trainer)
        {
            DbContext.Trainers.Add(trainer);
            DbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]

        public IActionResult Update(int id)
        {
            var result = DbContext.Trainers.Find(id);
            return View(result);
        }

        [HttpPost]

        public IActionResult Update(Trainer trainer)
        {
            DbContext.Update(trainer);
            DbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var result = DbContext.Trainers.Find(id);
            return View(result);
        }

        [HttpPost]

        public IActionResult Delete(Trainer trainer)
        {
            DbContext.Remove(trainer);
            DbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

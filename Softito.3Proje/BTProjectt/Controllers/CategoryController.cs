using Data;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace BTProjectt.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class CategoryController : Controller
    {
        public readonly AppDbContext Context;

        public CategoryController(AppDbContext context)
        {
            this.Context = context;
        }

        [HttpGet]
        public IActionResult Index(string search)
        {
            var query = Context.Categories.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.Name.Contains(search) || c.Description.Contains(search));
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
        public IActionResult Create(Category category)
        {
            Context.Categories.Add(category);
            Context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var result = Context.Categories.Find(id);
            return View(result);
        }

        [HttpPost]

        public IActionResult Update(Category category)
        {
            Context.Update(category);
            Context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]

        public IActionResult Delete(int id)
        {
            var result = Context.Categories.Find(id);
            return View(result);
        }

        [HttpPost]

        public IActionResult Delete(Category category)
        {
            Context.Remove(category);
            Context.SaveChanges();
            return RedirectToAction("Index");
        }






    }
}

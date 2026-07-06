using Microsoft.AspNetCore.Mvc;
using projectcodefrst.Models;

namespace projectcodefrst.Controllers
{
    public class YazarController : Controller
    {
        public readonly KutuphaneContext dbcontext;

        public YazarController(KutuphaneContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public IActionResult Index()
        {
            var result = dbcontext.Yazars.ToList();
            return View(result);
        }

        public IActionResult Search(string search)
        {
            var query = dbcontext.Yazars.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(y => y.YazarAdi.Contains(search));
            }
            var result = query.ToList();
            return View("Index", result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Yazar yazar)
        {
            dbcontext.Yazars.Add(yazar);
            dbcontext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]

        public IActionResult Update(int id)
        {
            var result = dbcontext.Yazars.Find(id);
            return View(result);
        }

        [HttpPost]

        public IActionResult Update(Yazar yazar)
        {
            dbcontext.Update(yazar);
            dbcontext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var result = dbcontext.Yazars.Find(id);
            return View(result);
        }

        [HttpPost]

        public IActionResult Delete(Yazar yazar)
        {
            dbcontext.Remove(yazar);
            dbcontext.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}

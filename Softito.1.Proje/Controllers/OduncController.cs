using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projectcodefrst.Models;

namespace projectcodefrst.Controllers
{
    public class OduncController : Controller
    {
        public readonly KutuphaneContext dbcontext;

        public OduncController(KutuphaneContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public IActionResult Index()
        {
            var result = dbcontext.Oduncs.Include(o => o.Kitap).ToList();
            return View(result);
        }

        public IActionResult Search(string search)
        {
            var query = dbcontext.Oduncs.Include(o => o.Kitap).AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(o => o.AlanKisi.Contains(search) || o.Kitap.KitapAdi.Contains(search));
            }
            var result = query.ToList();
            return View("Index", result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.KitapId = new SelectList(dbcontext.Kitaps.ToList(), "KitapId", "KitapAdi");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Odunc odunc)
        {
            dbcontext.Oduncs.Add(odunc);
            dbcontext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var result = dbcontext.Oduncs.Include(o => o.Kitap).FirstOrDefault(o => o.OduncId == id);
            if (result == null)
            {
                return NotFound();
            }
            ViewBag.KitapId = new SelectList(dbcontext.Kitaps.ToList(), "KitapId", "KitapAdi", result.KitapId);
            return View(result);
        }

        [HttpPost]
        public IActionResult Update(Odunc odunc)
        {
            dbcontext.Update(odunc);
            dbcontext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var result = dbcontext.Oduncs.Include(o => o.Kitap).FirstOrDefault(o => o.OduncId == id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        [HttpPost]
        public IActionResult Delete(Odunc odunc)
        {
            dbcontext.Remove(odunc);
            dbcontext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

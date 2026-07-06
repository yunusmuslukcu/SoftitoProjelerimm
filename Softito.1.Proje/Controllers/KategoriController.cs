using Microsoft.AspNetCore.Mvc;
using projectcodefrst.Models;

namespace projectcodefrst.Controllers
{
    public class KategoriController : Controller
    {



        public readonly KutuphaneContext dbcontext;

        public KategoriController(KutuphaneContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public IActionResult Index()
        {
            var result = dbcontext.Kategoris.ToList();
            return View(result);
        }

        public IActionResult Search(string search)
        {
            var query = dbcontext.Kategoris.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(k => k.KategoriAdi.Contains(search));
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
        public IActionResult Create(Kategori kategori)
        {
            dbcontext.Kategoris.Add(kategori);
            dbcontext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]

        public IActionResult Update(int id)
        {
            var result = dbcontext.Kategoris.Find(id);
            return View(result);
        }

        [HttpPost]

        public IActionResult Update(Kategori kategori)
        {
            dbcontext.Update(kategori);
            dbcontext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var result = dbcontext.Kategoris.Find(id);
            return View(result);
        }

        [HttpPost]

        public IActionResult Delete(Kategori kategori)
        {
            dbcontext.Remove(kategori);
            dbcontext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

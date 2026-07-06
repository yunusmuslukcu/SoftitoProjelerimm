using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projectcodefrst.Models;

namespace projectcodefrst.Controllers
{
    public class KitapController : Controller
    {
        public readonly KutuphaneContext dbcontext;

        public KitapController(KutuphaneContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public IActionResult Index()
        {
            var result = dbcontext.Kitaps.Include(k => k.Yazar).Include(k => k.Kategori).ToList();
            return View(result);
        }

        public IActionResult Search(string search)
        {
            var query = dbcontext.Kitaps.Include(k => k.Yazar).Include(k => k.Kategori).AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(k => k.KitapAdi.Contains(search) || k.Yazar.YazarAdi.Contains(search) || k.Kategori.KategoriAdi.Contains(search));
            }
            var result = query.ToList();
            return View("Index", result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.YazarId = new SelectList(dbcontext.Yazars.ToList(), "YazarId", "YazarAdi");
            ViewBag.KategoriID = new SelectList(dbcontext.Kategoris.ToList(), "KategoriId", "KategoriAdi");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Kitap kitap)
        {
            dbcontext.Kitaps.Add(kitap);
            dbcontext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var result = dbcontext.Kitaps.Include(k => k.Yazar).Include(k => k.Kategori).FirstOrDefault(k => k.KitapId == id);
            if (result == null)
            {
                return NotFound();
            }
            ViewBag.YazarId = new SelectList(dbcontext.Yazars.ToList(), "YazarId", "YazarAdi", result.YazarId);
            ViewBag.KategoriID = new SelectList(dbcontext.Kategoris.ToList(), "KategoriId", "KategoriAdi", result.KategoriID);
            return View(result);
        }

        [HttpPost]
        public IActionResult Update(Kitap kitap)
        {
            dbcontext.Update(kitap);
            dbcontext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var result = dbcontext.Kitaps.Include(k => k.Yazar).Include(k => k.Kategori).FirstOrDefault(k => k.KitapId == id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        [HttpPost]
        public IActionResult Delete(Kitap kitap)
        {
            dbcontext.Remove(kitap);
            dbcontext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

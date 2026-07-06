using Microsoft.AspNetCore.Mvc;
using projectcodefrst.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace projectcodefrst.Controllers
{
    public class RaporController : Controller
    {
        private readonly KutuphaneContext _context;

        public RaporController(KutuphaneContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var model = new RaporViewModel();

            model.ToplamKitap = _context.Kitaps.Count();
            model.ToplamYazar = _context.Yazars.Count();
            model.ToplamKategori = _context.Kategoris.Count();
            model.ToplamOdunc = _context.Oduncs.Count();

            model.OrtalamaSayfaSayisi = _context.Kitaps.Any() ? _context.Kitaps.Average(k => k.Sayfa) : 0;
            model.EnKalinKitap = _context.Kitaps.Include(k => k.Yazar).Include(k => k.Kategori).OrderByDescending(k => k.Sayfa).FirstOrDefault();

            model.KategoriDagilimi = _context.Kitaps
                .GroupBy(k => k.Kategori.KategoriAdi)
                .Select(g => new KategoriRaporDto
                {
                    KategoriAdi = g.Key ?? "Kategorisiz",
                    KitapSayisi = g.Count()
                })
                .ToList();

            model.YazarDagilimi = _context.Kitaps
                .GroupBy(k => k.Yazar.YazarAdi)
                .Select(g => new YazarRaporDto
                {
                    YazarAdi = g.Key ?? "Yazarsız",
                    KitapSayisi = g.Count()
                })
                .ToList();

            // 1. INNER JOIN Sorgusu (Kitap ve Yazarlar)
            model.InnerJoinResult = (from k in _context.Kitaps
                                     join y in _context.Yazars on k.YazarId equals y.YazarId
                                     select new BookAuthorInnerJoinDto
                                     {
                                         KitapAdi = k.KitapAdi,
                                         YazarAdi = y.YazarAdi,
                                         Sayfa = k.Sayfa
                                     }).ToList();

            // 2. LEFT JOIN Sorgusu (Yazar ve Kitaplar)
            model.LeftJoinResult = (from y in _context.Yazars
                                    join k in _context.Kitaps on y.YazarId equals k.YazarId into joinGroup
                                    from subKitap in joinGroup.DefaultIfEmpty()
                                    select new AuthorBookLeftJoinDto
                                    {
                                        YazarAdi = y.YazarAdi,
                                        KitapAdi = subKitap != null ? subKitap.KitapAdi : "Kitabı Yok",
                                        Sayfa = subKitap != null ? subKitap.Sayfa : 0
                                    }).ToList();

            // 3. GROUP BY Sorgusu (Kategorilere göre kitap sayısı ve toplam sayfa)
            model.GroupByResult = (from k in _context.Kitaps
                                   group k by k.Kategori.KategoriAdi into g
                                   select new CategoryPagesGroupByDto
                                   {
                                       KategoriAdi = g.Key ?? "Kategorisiz",
                                       ToplamSayfa = g.Sum(x => x.Sayfa),
                                       KitapSayisi = g.Count()
                                   }).ToList();

            // 4. ORDER BY Sorgusu (Sayfa sayısına göre azalan kitap listesi)
            model.OrderByResult = (from k in _context.Kitaps
                                   orderby k.Sayfa descending
                                   select new BookOrderByDto
                                   {
                                       KitapAdi = k.KitapAdi,
                                       YazarAdi = k.Yazar.YazarAdi,
                                       Sayfa = k.Sayfa
                                   }).ToList();

            return View(model);
        }
    }
}

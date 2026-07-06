using GymAppLite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;

namespace GymAppLite.Controllers
{
    public class HomeController : Controller
    {
        private readonly GymContext DbContext;

        public HomeController(GymContext dbContext)
        {
            this.DbContext = dbContext;
        }

        [AllowAnonymous]
        public IActionResult Landing()
        {
            ViewBag.ActiveMembersCount = DbContext.Members.Count();
            ViewBag.TrainersCount = DbContext.Trainers.Count();
            ViewBag.ProgressLogsCount = DbContext.ProgressLogs.Count();
            ViewBag.Packages = DbContext.SubscriptionTypes.ToList();
            ViewBag.TopTrainers = DbContext.Trainers.Take(4).ToList();
            return View();
        }

        public IActionResult Index()
        {
            // 1. Üst Panel İstatistikleri (Dinamik)
            ViewBag.ActiveMembersCount = DbContext.Members.Count();
            ViewBag.TrainersCount = DbContext.Trainers.Count();
            ViewBag.ProgressLogsCount = DbContext.ProgressLogs.Count();
            ViewBag.MonthlyRevenue = DbContext.Members.Include(m => m.SubscriptionType).Sum(m => m.SubscriptionType.Price);

            // 2. Grafik Verisi (Paket türlerine göre üye adetleri)
            var packageStats = DbContext.SubscriptionTypes
                .Select(st => new {
                    PackageName = st.TypeName,
                    MemberCount = st.Members.Count()
                }).ToList();

            ViewBag.ChartLabels = packageStats.Select(x => x.PackageName).ToArray();
            ViewBag.ChartData = packageStats.Select(x => x.MemberCount).ToArray();

            // 3. Son 5 Üye Kaydı (Proxy: Id DESC)
            ViewBag.RecentMembers = DbContext.Members
                .Include(m => m.SubscriptionType)
                .Include(m => m.Trainer)
                .OrderByDescending(m => m.Id)
                .Take(5)
                .ToList();

            // 4. Son 5 Gelişim Programı (ProgressLogs)
            ViewBag.RecentProgressLogs = DbContext.ProgressLogs
                .Include(p => p.Member)
                .OrderByDescending(p => p.Id)
                .Take(5)
                .ToList();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

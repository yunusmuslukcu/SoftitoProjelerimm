using GymAppLite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GymAppLite.Controllers
{
    public class RaporController : Controller
    {
        private readonly GymContext DbContext;

        public RaporController(GymContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public IActionResult Index()
        {
            // 1. COUNT ve İstatistikler
            ViewBag.TotalMembers = DbContext.Members.Count();
            ViewBag.TotalTrainers = DbContext.Trainers.Count();
            ViewBag.TotalProgressLogs = DbContext.ProgressLogs.Count();
            ViewBag.TotalPackages = DbContext.SubscriptionTypes.Count();

            // 2. ORDER BY + INCLUDE kullanımı (Üyeleri İsme Göre Sıralama)
            var sortedMembers = DbContext.Members
                .Include(m => m.SubscriptionType)
                .Include(m => m.Trainer)
                .OrderBy(m => m.FirstName)
                .ThenBy(m => m.LastName)
                .ToList();
            ViewBag.SortedMembers = sortedMembers;

            // 3. GROUP BY + COUNT kullanımı (Paketlere Göre Üye Sayıları)
            var membersByPackage = DbContext.Members
                .Include(m => m.SubscriptionType)
                .GroupBy(m => m.SubscriptionType.TypeName)
                .Select(g => new GroupedReportItem
                {
                    KeyName = g.Key ?? "Paket Tanımsız",
                    Count = g.Count()
                })
                .ToList();
            ViewBag.MembersByPackage = membersByPackage;

            // 4. INNER JOIN kullanımı (Üye ve Atanmış Antrenör eşleşmesi)
            var innerJoinResult = DbContext.Members
                .Join(
                    DbContext.Trainers,
                    member => member.TrainerId,
                    trainer => trainer.Id,
                    (member, trainer) => new MemberTrainerJoinItem
                    {
                        MemberName = member.FirstName + " " + member.LastName,
                        TrainerName = trainer.FullName
                    }
                )
                .ToList();
            ViewBag.InnerJoinResult = innerJoinResult;

            // 5. LEFT JOIN kullanımı (Tüm Üyeler ve Antrenör Durumları)
            var leftJoinResult = DbContext.Members
                .GroupJoin(
                    DbContext.Trainers,
                    member => member.TrainerId,
                    trainer => trainer.Id,
                    (member, trainers) => new { member, trainers }
                )
                .SelectMany(
                    x => x.trainers.DefaultIfEmpty(),
                    (x, trainer) => new MemberTrainerJoinItem
                    {
                        MemberName = x.member.FirstName + " " + x.member.LastName,
                        TrainerName = trainer != null ? trainer.FullName : "Antrenör Atanmadı"
                    }
                )
                .ToList();
            ViewBag.LeftJoinResult = leftJoinResult;

            return View();
        }
    }

    public class GroupedReportItem
    {
        public string KeyName { get; set; } = null!;
        public int Count { get; set; }
    }

    public class MemberTrainerJoinItem
    {
        public string MemberName { get; set; } = null!;
        public string TrainerName { get; set; } = null!;
    }
}

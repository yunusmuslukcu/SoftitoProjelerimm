using BTProjectt.Models;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;

namespace BTProjectt.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class HomeController : Controller
    {
        private readonly AppDbContext Context;

        public HomeController(AppDbContext context)
        {
            this.Context = context;
        }

        public IActionResult Index()
        {
            var viewModel = new DashboardViewModel
            {
                TotalTickets = Context.Tickets.Count(),
                OpenTickets = Context.Tickets.Count(t => t.Status == Model.TicketStatus.Open),
                InProgressTickets = Context.Tickets.Count(t => t.Status == Model.TicketStatus.InProgress),
                ResolvedTickets = Context.Tickets.Count(t => t.Status == Model.TicketStatus.Resolved),
                TotalCategories = Context.Categories.Count(),
                TotalUsers = Context.Users.Count(),
                RecentTickets = Context.Tickets
                    .Include(t => t.Category)
                    .Include(t => t.User)
                    .OrderByDescending(t => t.Id)
                    .Take(5)
                    .ToList()
            };

            // Kategori bazlı bilet dağılımı (Chart.js için)
            var categoryData = Context.Categories
                .Select(c => new {
                    Name = c.Name,
                    Count = c.Tickets.Count()
                })
                .ToList();

            viewModel.CategoryNames = categoryData.Select(x => x.Name).ToList();
            viewModel.CategoryTicketCounts = categoryData.Select(x => x.Count).ToList();

            return View(viewModel);
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

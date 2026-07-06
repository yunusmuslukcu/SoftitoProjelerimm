using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BTProjectt.Models;
using System.Linq;

namespace BTProjectt.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class RaporController : Controller
    {
        private readonly AppDbContext Context;

        public RaporController(AppDbContext context)
        {
            this.Context = context;
        }

        public IActionResult Index()
        {
            var viewModel = new RaporViewModel();

            // 1. COUNT SORGU
            viewModel.TotalTickets = Context.Tickets.Count();
            viewModel.TotalCategories = Context.Categories.Count();
            viewModel.TotalUsers = Context.Users.Count();

            // 2. INNER JOIN SORGU
            // Ticket + Category + User tablolarını birleştiriyoruz
            viewModel.InnerJoinData = (from t in Context.Tickets
                                       join c in Context.Categories on t.CategoryId equals c.Id
                                       join u in Context.Users on t.UserId equals u.Id
                                       select new InnerJoinDto
                                       {
                                           TicketTitle = t.Title,
                                           CategoryName = c.Name,
                                           UserName = u.FullName,
                                           Status = t.Status
                                       }).ToList();

            // 3. LEFT JOIN SORGU
            // Kategorileri ve varsa ilişkili Ticket'larını listeliyoruz (Ticket'ı olmayan kategoriler de gelmeli)
            viewModel.LeftJoinData = (from c in Context.Categories
                                      join t in Context.Tickets on c.Id equals t.CategoryId into ticketGroup
                                      from subTicket in ticketGroup.DefaultIfEmpty()
                                      select new LeftJoinDto
                                      {
                                          CategoryName = c.Name,
                                          TicketTitle = subTicket != null ? subTicket.Title : "Talep Yok"
                                      }).ToList();

            // 4. ORDER BY SORGU
            // Destek taleplerini ID numarasına göre tersten sıralıyoruz (Son eklenenler ilk başta)
            viewModel.OrderByData = Context.Tickets
                                           .OrderByDescending(t => t.Id)
                                           .Select(t => new OrderByDto
                                           {
                                               TicketId = t.Id,
                                               TicketTitle = t.Title
                                           })
                                           .ToList();

            // 5. GROUP BY SORGU
            // Talepleri durumlarına (Status) göre gruplayıp adetlerini buluyoruz
            viewModel.GroupByData = Context.Tickets
                                           .GroupBy(t => t.Status)
                                           .Select(g => new GroupByDto
                                           {
                                               Status = g.Key,
                                               Count = g.Count()
                                           })
                                           .ToList();

            return View(viewModel);
        }
    }
}

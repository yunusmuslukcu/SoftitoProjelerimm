using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;

namespace BTProjectt.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class TicketController : Controller
    {
        public readonly AppDbContext Context;



        public TicketController(AppDbContext context)
        {
            this.Context = context;
        }

        [HttpGet]
        public IActionResult Index(string search)
        {
            var query = Context.Tickets
                .Include(t => t.Category)
                .Include(t => t.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(t => t.Title.Contains(search) || 
                                         t.Description.Contains(search) || 
                                         t.Category.Name.Contains(search) || 
                                         t.User.FullName.Contains(search));
            }

            var result = query.ToList();
            return View(result);
        }


        [HttpGet]

        public IActionResult Create()
        {
            ViewBag.Categories = Context.Categories.ToList();  //kategroileri seçilir liste getirebilmek için!
            ViewBag.Users = Context.Users.ToList();            //kullanıcıları seçilir liste getirebilmek için!
            return View();                                     //Bu zaten genel olarak ticket verisini getirecek!
        }

        [HttpPost]

        public IActionResult Create(Ticket ticket)
        {
            Context.Tickets.Add(ticket);  //Ekle
            Context.SaveChanges();        //Kaydet
            return RedirectToAction("Index");  //Ekleme ve kaydetme işlemi bttikten sonra ındex'e yönlendir!
        }

        [HttpGet]

        public IActionResult Update(int id)
        {
            var ticket = Context.Tickets.Find(id);
            ViewBag.Categories = Context.Categories.ToList();
            ViewBag.Users = Context.Users.ToList();
            return View(ticket);
        }

        [HttpPost]

        public IActionResult Update(Ticket ticket)
        {
            Context.Tickets.Update(ticket);
            Context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpDelete]

        public IActionResult Delete(int id)
        {
            var ticket = Context.Tickets
                                .Include(t => t.Category)
                                .Include(t => t.User)
                                .FirstOrDefault(t => t.Id == id);

            return View(ticket);
        }

        [HttpPost]
        public IActionResult Delete(Ticket ticket)
        {
            var originalUser = Context.Tickets.Find(ticket.Id);
            this.Context.Tickets.Remove(originalUser);
            this.Context.SaveChanges();
            return RedirectToAction("Index");
        }




    }
}

using Data;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace BTProjectt.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class UserController : Controller
    {
        public readonly AppDbContext Context;

        public UserController(AppDbContext context)
        {
            this.Context = context;
        }

        [HttpGet]
        public IActionResult Index(string search)
        {
            var query = Context.Users.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(u => u.FullName.Contains(search) || u.Email.Contains(search) || u.Department.Contains(search));
            }
            var result = query.ToList();
            return View(result);
        }

        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Create(User user)
        {
            Context.Users.Add(user);
            Context.SaveChanges();
            return RedirectToAction("Index");

        }

        [HttpGet]

        public IActionResult Update(int id)
        {
            var result = Context.Users.Find(id);
            return View(result);
        }

        [HttpPost]

        public IActionResult Update(User user)
        {
            Context.Users.Update(user);
            Context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]

        public IActionResult Delete(int id)
        {
            var result = Context.Users.Find(id);
            return View(result);
        }

        [HttpPost]
        public IActionResult Delete(User user)
        {
            var originalUser = Context.Users.Find(user.Id);
            this.Context.Users.Remove(originalUser);
            this.Context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

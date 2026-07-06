using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Data;

namespace Marketing.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AppUserController : Controller
    {
        private readonly AppDbContext _context;
        public AppUserController(AppDbContext context) { _context = context; }
        public IActionResult Index() => View(_context.Users.ToList());
    }
}

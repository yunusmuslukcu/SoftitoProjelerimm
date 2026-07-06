using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Data;
using Model;

namespace Marketing.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class BranchController : Controller
    {
        private readonly AppDbContext _context;

        public BranchController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Branches.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Branch branch)
        {
            if (ModelState.IsValid)
            {
                _context.Branches.Add(branch);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(branch);
        }
    }
}

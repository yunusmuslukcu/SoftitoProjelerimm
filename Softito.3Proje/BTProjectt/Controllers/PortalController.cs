using Microsoft.AspNetCore.Mvc;

namespace BTProjectt.Controllers
{
    public class PortalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

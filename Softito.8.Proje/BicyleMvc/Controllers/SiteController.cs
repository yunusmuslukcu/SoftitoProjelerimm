using Microsoft.AspNetCore.Mvc;

namespace BicyleMvc.Controllers
{
    public class SiteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

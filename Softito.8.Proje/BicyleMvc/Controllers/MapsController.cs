using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BicyleMvc.Controllers
{
    [Authorize]
    public class MapsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

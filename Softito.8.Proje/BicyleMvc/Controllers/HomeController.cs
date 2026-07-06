using BicyleMvc.Models;
using BicyleMvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BicyleMvc.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApiService _apiService;

        public HomeController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // --- DASHBOARD (PANEL) ---
        public async Task<IActionResult> Index()
        {
            var stations = await _apiService.GetAllStationsAsync();
            var rentals = await _apiService.GetAllRentalsAsync();

            // İstatistikleri hesapla
            ViewBag.TotalStations = stations.Count;
            ViewBag.ActiveStations = stations.Count(s => s.IsActive);
            ViewBag.TotalCapacity = stations.Sum(s => s.Capacity);
            ViewBag.ActiveBicycles = stations.Sum(s => s.ActiveBicyclesCount);
            
            ViewBag.TotalRentals = rentals.Count;
            ViewBag.OngoingRentals = rentals.Count(r => !r.RentalEndTime.HasValue);
            ViewBag.CompletedRentals = rentals.Count(r => r.RentalEndTime.HasValue);
            ViewBag.TotalRevenue = rentals.Where(r => r.TotalPrice.HasValue).Sum(r => r.TotalPrice.Value);

            return View();
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

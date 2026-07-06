using BicyleMvc.Models;
using BicyleMvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BicyleMvc.Controllers
{
    [Authorize]
    public class StationsController : Controller
    {
        private readonly ApiService _apiService;

        public StationsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: /Stations
        public async Task<IActionResult> Index(string searchQuery)
        {
            var stations = await _apiService.GetAllStationsAsync();

            // Arama filtresi
            if (!string.IsNullOrEmpty(searchQuery))
            {
                var lowerQuery = searchQuery.ToLower();
                stations = stations.Where(s => 
                    (s.StationName != null && s.StationName.ToLower().Contains(lowerQuery)) || 
                    (s.Location != null && s.Location.ToLower().Contains(lowerQuery))
                ).ToList();
            }

            ViewBag.SearchQuery = searchQuery;
            return View(stations);
        }

        // POST: /Stations/SaveStation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveStation(StationViewModel station)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "İstasyon bilgileri geçersiz!";
                return RedirectToAction(nameof(Index));
            }

            var (isSuccess, message) = await _apiService.SaveOrUpdateStationAsync(station);

            if (isSuccess)
            {
                TempData["SuccessMessage"] = message;
            }
            else
            {
                TempData["ErrorMessage"] = message;
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: /Stations/DeleteStation
        [HttpPost]
        public async Task<IActionResult> DeleteStation(int id)
        {
            var (isSuccess, message) = await _apiService.DeleteStationAsync(id);

            if (isSuccess)
            {
                TempData["SuccessMessage"] = message;
            }
            else
            {
                TempData["ErrorMessage"] = message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

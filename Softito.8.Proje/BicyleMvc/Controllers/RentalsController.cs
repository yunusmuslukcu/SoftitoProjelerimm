using BicyleMvc.Models;
using BicyleMvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BicyleMvc.Controllers
{
    [Authorize]
    public class RentalsController : Controller
    {
        private readonly ApiService _apiService;

        public RentalsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: /Rentals
        public async Task<IActionResult> Index(string searchQuery)
        {
            var rentals = await _apiService.GetAllRentalsAsync();
            var stations = await _apiService.GetAllStationsAsync();
            var users = await _apiService.GetAllUsersAsync();

            var stationDict = stations.ToDictionary(s => s.Id, s => s.StationName);
            var userDict = users.ToDictionary(u => u.Id, u => u.Email);

            // İlişkili alanları doldur
            foreach (var rental in rentals)
            {
                if (stationDict.TryGetValue(rental.StationId, out var stationName))
                {
                    rental.StationName = stationName;
                }
                else
                {
                    rental.StationName = "Bilinmeyen İstasyon";
                }

                if (userDict.TryGetValue(rental.UserId, out var userEmail))
                {
                    rental.UserEmail = userEmail;
                }
            }

            // Arama filtresi
            if (!string.IsNullOrEmpty(searchQuery))
            {
                var lowerQuery = searchQuery.ToLower();
                rentals = rentals.Where(r => 
                    (r.StationName != null && r.StationName.ToLower().Contains(lowerQuery)) ||
                    r.UserId.ToString().Contains(lowerQuery) ||
                    (r.UserEmail != null && r.UserEmail.ToLower().Contains(lowerQuery))
                ).ToList();
            }

            ViewBag.SearchQuery = searchQuery;
            ViewBag.Stations = stations; // Yeni kiralama modalındaki açılır kutu için
            ViewBag.Users = users; // Kullanıcı seçimi için
            return View(rentals);
        }

        // POST: /Rentals/StartRental
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartRental(RentalViewModel rental)
        {
            // Kiralama başlangıcı için varsayılan alanlar
            rental.RentalStartTime = DateTime.Now;
            rental.RentalEndTime = null;
            rental.TotalPrice = null;

            var (isSuccess, message) = await _apiService.SaveOrUpdateRentalAsync(rental);

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

        // POST: /Rentals/ReturnRental
        [HttpPost]
        public async Task<IActionResult> ReturnRental(int id)
        {
            var rental = await _apiService.GetRentalByIdAsync(id);
            if (rental == null)
            {
                TempData["ErrorMessage"] = "Kiralama kaydı bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            rental.RentalEndTime = DateTime.Now;
            
            // Ücret hesabı: Örnek olarak saatlik 30 TL, en az 30 TL
            var duration = rental.RentalEndTime.Value - rental.RentalStartTime;
            double hours = Math.Ceiling(duration.TotalHours);
            if (hours < 1) hours = 1;
            rental.TotalPrice = (decimal)(hours * 30.0);

            var (isSuccess, message) = await _apiService.SaveOrUpdateRentalAsync(rental);

            if (isSuccess)
            {
                TempData["SuccessMessage"] = "Bisiklet başarıyla teslim alındı. Toplam Ücret: " + rental.TotalPrice + " TL";
            }
            else
            {
                TempData["ErrorMessage"] = message;
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: /Rentals/DeleteRental
        [HttpPost]
        public async Task<IActionResult> DeleteRental(int id)
        {
            var (isSuccess, message) = await _apiService.DeleteRentalAsync(id);

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

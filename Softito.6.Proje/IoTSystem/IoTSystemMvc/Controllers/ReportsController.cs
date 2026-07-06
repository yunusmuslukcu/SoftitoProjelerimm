using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using IoTSystemMvc.Models;

using Microsoft.AspNetCore.Authorization;

namespace IoTSystemMvc.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly string _apiUrl = "http://localhost:5016/api/Reports";

        [HttpGet]
        public IActionResult Index()
        {
            var response = _client.GetAsync($"{_apiUrl}/GetReportData").Result;

            if (response.IsSuccessStatusCode)
            {
                string jsonData = response.Content.ReadAsStringAsync().Result;
                var reports = JsonConvert.DeserializeObject<ReportsViewModel>(jsonData);
                return View(reports);
            }
            return View(new ReportsViewModel());
        }
    }
}

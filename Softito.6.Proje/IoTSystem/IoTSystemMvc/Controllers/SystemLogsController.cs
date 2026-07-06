using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using IoTSystemMvc.Models;
using System.Text;

using Microsoft.AspNetCore.Authorization;

namespace IoTSystemMvc.Controllers
{
    [Authorize]
    public class SystemLogsController : Controller
    {

        private readonly HttpClient _client = new HttpClient();
        private readonly string _apiUrl = "http://localhost:5016/api/SystemLogs";


        [HttpGet]
        public IActionResult Index(string searchString)
        {
            var response = _client.GetAsync($"{_apiUrl}/GetSystemLogs").Result;

            if (response.IsSuccessStatusCode)
            {
                string jsonData = response.Content.ReadAsStringAsync().Result;
                List<SystemLogViewModel> logs = JsonConvert.DeserializeObject<List<SystemLogViewModel>>(jsonData);

                if (searchString != null)
                {
                    logs = logs.Where(l => l.LogCode.Contains(searchString, StringComparison.OrdinalIgnoreCase) || 
                                           l.LogLevel.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                                           l.Description.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                ViewData["CurrentFilter"] = searchString;
                return View(logs);
            }
            return View(new List<SystemLogViewModel>());
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View(new SystemLogViewModel());
        }

        [HttpPost]
        public IActionResult Create(SystemLogViewModel model)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var response = _client.PostAsync($"{_apiUrl}/AddSystemLog", content).Result;
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var response = _client.GetAsync($"{_apiUrl}/GetSystemLogById/{id}").Result;

            var log = JsonConvert.DeserializeObject<SystemLogViewModel>(response.Content.ReadAsStringAsync().Result);
            return View(log);
        }

        [HttpPost]
        public IActionResult Edit(SystemLogViewModel model)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var response = _client.PutAsync($"{_apiUrl}/UpdateSystemLog/{model.Id}", content).Result;
            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            var response = _client.DeleteAsync($"{_apiUrl}/DeleteSystemLog/{id}").Result;
            return RedirectToAction("Index");
        }
    }
}
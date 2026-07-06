using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using IoTSystemMvc.Models;
using System.Text;

using Microsoft.AspNetCore.Authorization;

namespace IoTSystemMvc.Controllers
{
    [Authorize]
    public class DevicesController : Controller
    {

        private readonly HttpClient _client = new HttpClient();
        private readonly string _apiUrl = "http://localhost:5016/api/Devices";


        [HttpGet]
        public IActionResult Index(string searchString)
        {
            var response = _client.GetAsync($"{_apiUrl}/GetDevices").Result;

            if (response.IsSuccessStatusCode)
            {
                string jsonData = response.Content.ReadAsStringAsync().Result;
                List<DeviceViewModel> devices = JsonConvert.DeserializeObject<List<DeviceViewModel>>(jsonData);

                if (searchString != null)
                {
                    devices = devices.Where(d => d.DeviceName.Contains(searchString, StringComparison.OrdinalIgnoreCase) || 
                                                 d.IpAddress.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                                                 d.Location.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                ViewData["CurrentFilter"] = searchString;
                return View(devices);
            }
            return View(new List<DeviceViewModel>());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new DeviceViewModel());
        }

        [HttpPost]
        public IActionResult Create(DeviceViewModel model)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var response = _client.PostAsync($"{_apiUrl}/AddDevice", content).Result;
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var response = _client.GetAsync($"{_apiUrl}/GetDeviceById/{id}").Result;

            var device = JsonConvert.DeserializeObject<DeviceViewModel>(response.Content.ReadAsStringAsync().Result);
            return View(device);
        }

        [HttpPost]
        public IActionResult Edit(DeviceViewModel model)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var response = _client.PutAsync($"{_apiUrl}/UpdateDevice/{model.Id}", content).Result;
            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            var response = _client.DeleteAsync($"{_apiUrl}/DeleteDevice/{id}").Result;
            return RedirectToAction("Index");
        }
    }
}
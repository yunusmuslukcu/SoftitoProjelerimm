using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using IoTSystemMvc.Models;
using System.Text;

using Microsoft.AspNetCore.Authorization;

namespace IoTSystemMvc.Controllers
{
    [Authorize]
    public class SuppliersController : Controller
    {

        private readonly HttpClient _client = new HttpClient();
        private readonly string _apiUrl = "http://localhost:5016/api/Suppliers";


        [HttpGet]
        public IActionResult Index(string searchString)
        {
            var response = _client.GetAsync($"{_apiUrl}/GetSuppliers").Result;

            if (response.IsSuccessStatusCode)
            {
                string jsonData = response.Content.ReadAsStringAsync().Result;
                List<SupplierViewModel> suppliers = JsonConvert.DeserializeObject<List<SupplierViewModel>>(jsonData);

                if (searchString != null)
                {
                    suppliers = suppliers.Where(s => s.SupplierName.Contains(searchString, StringComparison.OrdinalIgnoreCase) || 
                                                     s.ContactName.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                                                     s.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                ViewData["CurrentFilter"] = searchString;
                return View(suppliers);
            }
            return View(new List<SupplierViewModel>());
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View(new SupplierViewModel());
        }

        [HttpPost]
        public IActionResult Create(SupplierViewModel model)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var response = _client.PostAsync($"{_apiUrl}/AddSupplier", content).Result;
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var response = _client.GetAsync($"{_apiUrl}/GetSupplierById/{id}").Result;

            var supplier = JsonConvert.DeserializeObject<SupplierViewModel>(response.Content.ReadAsStringAsync().Result);
            return View(supplier);
        }

        [HttpPost]
        public IActionResult Edit(SupplierViewModel model)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var response = _client.PutAsync($"{_apiUrl}/UpdateSupplier/{model.Id}", content).Result;
            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            var response = _client.DeleteAsync($"{_apiUrl}/DeleteSupplier/{id}").Result;
            return RedirectToAction("Index");
        }
    }
}
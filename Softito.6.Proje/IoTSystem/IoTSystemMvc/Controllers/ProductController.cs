using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using IoTSystemMvc.Models;
using System.Text;

using Microsoft.AspNetCore.Authorization;

namespace IoTSystemMvc.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly string _apiUrl = "http://localhost:5016/api/Products";


        [HttpGet]
        public IActionResult Index(string searchString)
        {
            var response = _client.GetAsync($"{_apiUrl}/GetProducts").Result;

            if (response.IsSuccessStatusCode)
            {
                string jsonData = response.Content.ReadAsStringAsync().Result;
                List<ProductViewModel> products = JsonConvert.DeserializeObject<List<ProductViewModel>>(jsonData);
                
                if (searchString != null)
                {
                    products = products.Where(p => p.ProductName.Contains(searchString, StringComparison.OrdinalIgnoreCase) || 
                                                   p.ProductCode.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                ViewData["CurrentFilter"] = searchString;
                return View(products);
            }
            return View(new List<ProductViewModel>());
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View(new ProductViewModel());
        }

        [HttpPost]
        public IActionResult Create(ProductViewModel model)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var response = _client.PostAsync($"{_apiUrl}/AddProduct", content).Result;
            return RedirectToAction("Index");
        }



        [HttpGet]
        public IActionResult Edit(int id)
        {
            var response = _client.GetAsync($"{_apiUrl}/GetProductById/{id}").Result;

            var product = JsonConvert.DeserializeObject<ProductViewModel>(response.Content.ReadAsStringAsync().Result);
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(ProductViewModel model)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var response = _client.PutAsync($"{_apiUrl}/UpdateProduct/{model.Id}", content).Result;
            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            var response = _client.DeleteAsync($"{_apiUrl}/DeleteProduct/{id}").Result;
            return RedirectToAction("Index");
        }
    }
}
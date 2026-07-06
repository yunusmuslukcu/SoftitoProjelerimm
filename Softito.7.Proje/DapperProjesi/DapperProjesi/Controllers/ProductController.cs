using Dapper;
using DapperProjesi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DapperProjesi.Controllers
{
    public class ProductController : Controller
    {
        // 1. ÜRÜNLERİ LİSTELEME (Index)
        public IActionResult Index()
        {
            
            return View(Context.Listeleme<Products>("ProductViewAll"));
        }

        // 2. ÜRÜN EKLEME/GÜNCELLEME
        public IActionResult EY(int id = 0)
        {
            if (id == 0)
            {
                
                return View();
            }
            else
            {
                
                DynamicParameters param = new DynamicParameters();
                param.Add("@Id", id);

                
                return View(Context.Listeleme<Products>("ProductViewById", param).FirstOrDefault());
            }
        }

        // 3. ÜRÜN KAYDET/GÜNCELLE
        [HttpPost]
        public IActionResult EY(Products product)
        {
            DynamicParameters param = new DynamicParameters();

            
            param.Add("@Id", product.Id);
            param.Add("@ProductName", product.ProductName);
            param.Add("@Price", product.Price);
            param.Add("@Stock", product.Stock);
            param.Add("@IsActive", product.IsActive);

            
            Context.ExecuteReturn("ProductEY", param);

            return RedirectToAction("Index");
        }

        // 4. ÜRÜN SİLME 
        public IActionResult Delete(int id = 0)
        {
            if (id != 0)
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Id", id);

                
                Context.ExecuteReturn("ProductSil", param);
            }

            return RedirectToAction("Index");
        }
    }
}

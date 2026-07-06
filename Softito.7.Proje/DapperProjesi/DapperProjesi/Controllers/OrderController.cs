using Dapper;
using DapperProjesi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DapperProjesi.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {

            return View(Context.Listeleme<Orders>("OrderViewAll"));
        }

        // 2. SİPARİŞ EKLEME/GÜNCELLEME SAYFASI
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

                
                return View(Context.Listeleme<Orders>("OrderViewById", param).FirstOrDefault());
            }
        }

        // 3. SİPARİŞ KAYDET / GÜNCELLE (POST EY)
        [HttpPost]
        public IActionResult EY(Orders order)
        {
            DynamicParameters param = new DynamicParameters();

            
            param.Add("@Id", order.Id);
            param.Add("@UserId", order.UserId);
            param.Add("@ProductId", order.ProductId);
            param.Add("@WalletId", order.WalletId);
            param.Add("@OrderNumber", order.OrderNumber);
            param.Add("@Quantity", order.Quantity);
            param.Add("@TotalPrice", order.TotalPrice);

            
            Context.ExecuteReturn("OrderEY", param);

            return RedirectToAction("Index");
        }

        // 4. SİPARİŞ SİLME (Delete)
        public IActionResult Delete(int id = 0)
        {
            if (id != 0)
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Id", id);

                
                Context.ExecuteReturn("OrderSil", param);
            }

            return RedirectToAction("Index");
        }
    }
}

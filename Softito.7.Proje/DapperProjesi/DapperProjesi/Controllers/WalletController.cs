using Dapper;
using DapperProjesi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DapperProjesi.Controllers
{
    public class WalletController : Controller
    {

        // 1. CÜZDANLARI LİSTELEME 
        public IActionResult Index()
        {
            
            return View(Context.Listeleme<Wallets>("WalletViewAll"));
        }

        // 2. CÜZDAN EKLEME / GÜNCELLEME SAYFASI (GET EY)
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

                
                return View(Context.Listeleme<Wallets>("WalletViewById", param).FirstOrDefault());
            }
        }

        // 3. CÜZDAN KAYDET/GÜNCELLE 
        [HttpPost]
        public IActionResult EY(Wallets wallet)
        {
            DynamicParameters param = new DynamicParameters();

            
            param.Add("@Id", wallet.Id);
            param.Add("@UserId", wallet.UserId);
            param.Add("@WalletNumber", wallet.WalletNumber);
            param.Add("@Balance", wallet.Balance);
            param.Add("@Currency", wallet.Currency);

            
            Context.ExecuteReturn("WalletEY", param);

            return RedirectToAction("Index");
        }

        // 4. CÜZDAN SİLME 
        public IActionResult Delete(int id = 0)
        {
            if (id != 0)
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Id", id);

                
                Context.ExecuteReturn("WalletSil", param);
            }

            return RedirectToAction("Index");
        }
    }
}

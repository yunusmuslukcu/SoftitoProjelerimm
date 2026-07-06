using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using IoTSystemMvc.Models;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace IoTSystemMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly string _apiUrl = "http://localhost:5016"; 


        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            StringContent content = new StringContent(JsonConvert.SerializeObject(new { email = model.Email, password = model.Password }), Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PostAsync($"{_apiUrl}/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Email)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                    
                    HttpContext.Session.SetString("IsLoggedIn", "True"); // Keep session support for backwards compatibility

                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Bağlantı Hatası: API ile iletişim kurulamadı. {ex.Message}");
                return View(model);
            }

            ModelState.AddModelError("", "E-posta veya şifre hatalı!");
            return View(model);
        }


        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            StringContent content = new StringContent(JsonConvert.SerializeObject(new { email = model.Email, password = model.Password }), Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PostAsync($"{_apiUrl}/register", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login");
                }

                // API'den dönen doğrulama hatalarını ayıkla (Şifre kriterleri vb.)
                var errorJson = await response.Content.ReadAsStringAsync();
                var errorObj = JsonConvert.DeserializeObject<IdentityErrorResponse>(errorJson);
                
                if (errorObj?.Errors != null && errorObj.Errors.Any())
                {
                    foreach (var err in errorObj.Errors)
                    {
                        foreach (var msg in err.Value)
                        {
                            // Türkçe çevirilerle hata mesajlarını güzelleştir
                            var localizedMsg = msg
                                .Replace("Passwords must be at least 6 characters.", "Şifre en az 6 karakter uzunluğunda olmalıdır.")
                                .Replace("Passwords must have at least one non alphanumeric character.", "Şifre en az bir özel karakter (örn: @, !, ?) içermelidir.")
                                .Replace("Passwords must have at least one digit ('0'-'9').", "Şifre en az bir rakam ('0'-'9') içermelidir.")
                                .Replace("Passwords must have at least one uppercase ('A'-'Z').", "Şifre en az bir büyük harf ('A'-'Z') içermelidir.")
                                .Replace("Passwords must have at least one lowercase ('a'-'z').", "Şifre en az bir küçük harf ('a'-'z') içermelidir.")
                                .Replace("is already taken.", "adresi zaten kayıtlı.");
                            
                            ModelState.AddModelError("", localizedMsg);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Kayıt işlemi başarısız oldu. Girdiğiniz bilgileri kontrol edin.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Bağlantı Hatası: API ile iletişim kurulamadı. {ex.Message}");
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TempData["Message"] = "Şifre sıfırlama bağlantısı başarıyla gönderildi.";
            return RedirectToAction("Login");
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("IsLoggedIn");
            return RedirectToAction("Login");
        }
    }

    // Identity API Hata Yanıt Modeli
    public class IdentityErrorResponse
    {
        public Dictionary<string, string[]> Errors { get; set; } = new();
    }
}
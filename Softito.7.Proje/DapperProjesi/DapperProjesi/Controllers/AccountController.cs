using DapperProjesi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Claims;


namespace WalletPayAdmin.Controllers
{
    public class AccountController : Controller
    {
        // 1. REGISTER (KAYIT OL) - GET
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // 2. REGISTER (KAYIT OL)
        [HttpPost]
        public IActionResult Register(Users model)
        {
            
            using (SqlConnection conn = new SqlConnection(Context.connectionString))
            {
                
                using (SqlCommand cmd = new SqlCommand("UserEY", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    
                    cmd.Parameters.AddWithValue("@Id", 0); 
                    cmd.Parameters.AddWithValue("@FullName", model.FullName);
                    cmd.Parameters.AddWithValue("@Email", model.Email);
                    cmd.Parameters.AddWithValue("@PasswordHash", model.PasswordHash); 
                    cmd.Parameters.AddWithValue("@Role", "User"); 

                    conn.Open();
                    cmd.ExecuteNonQuery(); 
                }
            }

            
            return RedirectToAction("Login");
        }

        // 3. LOGIN (GİRİŞ YAP)
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Login(string email, string passwordHash)
        {
            Users loggedInUser = null;

            using (SqlConnection conn = new SqlConnection(Context.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UserLoginKontrol", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) 
                        {
                            loggedInUser = new Users
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                FullName = reader["FullName"].ToString(),
                                Email = reader["Email"].ToString(),
                                Role = reader["Role"].ToString()
                            };
                        }
                    }
                }
            }

            if (loggedInUser != null)
            {
                
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, loggedInUser.Id.ToString()),
                    new Claim(ClaimTypes.Name, loggedInUser.FullName),
                    new Claim(ClaimTypes.Email, loggedInUser.Email),
                    new Claim(ClaimTypes.Role, loggedInUser.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                
                return RedirectToAction("Index", "Product");
            }

            
            ViewBag.ErrorMessage = "Hatalı E-posta veya Şifre!";
            return View();
        }

        // 5. LOGOUT (ÇIKIŞ YAP)
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
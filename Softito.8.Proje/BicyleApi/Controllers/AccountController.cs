using BicyleApi.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace BicyleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost("register")]
        public IActionResult Register(string firstName, string lastName, string email, string password)
        {
            
            var paramEmail = new DynamicParameters();
            paramEmail.Add("@Email", email);
            var existingUser = Context.Listeleme<User>("UsersGetByEmail", paramEmail).FirstOrDefault();

            if (existingUser != null)
                return BadRequest("Bu email adresi zaten kullanımda.");

            
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            
            var parameters = new DynamicParameters();
            parameters.Add("@Id", 0);
            parameters.Add("@FirstName", firstName);
            parameters.Add("@LastName", lastName);
            parameters.Add("@Email", email);
            parameters.Add("@PasswordHash", passwordHash);
            parameters.Add("@PasswordSalt", passwordSalt);
            parameters.Add("@Role", "Customer"); 

            Context.ExecuteReturn("UsersEY", parameters);

            return Ok(new { Message = "Kayıt işlemi başarıyla tamamlandı." });
        }

        // 2. LOGIN (GİRİŞ YAP)
        
        [HttpPost("login")]
        public IActionResult Login(string email, string password)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Email", email);

            var user = Context.Listeleme<User>("UsersGetByEmail", parameters).FirstOrDefault();

            
            if (user == null || !VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return BadRequest("Hatalı email veya şifre girdiniz.");

            
            return Ok(new
            {
                Message = "Giriş başarılı.",
                User = user
            });
        }

        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            var users = Context.QueryRaw<User>("SELECT * FROM Users");
            return Ok(users);
        }

        // --- KRİPTOGRAFİ YARDIMCI METOTLARI ---
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }
            return true;
        }
    }
}

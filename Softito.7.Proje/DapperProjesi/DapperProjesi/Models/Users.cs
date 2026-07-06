namespace DapperProjesi.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User"; // 'Admin' veya 'User'
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

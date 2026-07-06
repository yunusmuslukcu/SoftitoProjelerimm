using System.ComponentModel.DataAnnotations;

namespace BicyleMvc.Models
{
    public class RentalViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kullanıcı seçimi gereklidir.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "İstasyon seçimi gereklidir.")]
        public int StationId { get; set; }

        [Required(ErrorMessage = "Kiralama başlangıç saati gereklidir.")]
        public DateTime RentalStartTime { get; set; } = DateTime.Now;

        public DateTime? RentalEndTime { get; set; }

        public decimal? TotalPrice { get; set; }

        // Görünüm katmanında eşleme için yardımcı alanlar (MVC tarafında doldurulabilir)
        public string? StationName { get; set; }
        public string? UserEmail { get; set; }
    }
}

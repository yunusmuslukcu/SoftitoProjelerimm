using System.ComponentModel.DataAnnotations;

namespace BicyleMvc.Models
{
    public class StationViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "İstasyon adı gereklidir.")]
        [StringLength(100, ErrorMessage = "İstasyon adı en fazla 100 karakter olabilir.")]
        public string StationName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Konum alanı gereklidir.")]
        [StringLength(250, ErrorMessage = "Konum en fazla 250 karakter olabilir.")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kapasite alanı gereklidir.")]
        [Range(1, int.MaxValue, ErrorMessage = "Kapasite en az 1 olmalıdır.")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Aktif Bisiklet Sayısı gereklidir.")]
        [Range(0, int.MaxValue, ErrorMessage = "Aktif bisiklet sayısı sıfırdan küçük olamaz.")]
        public int ActiveBicyclesCount { get; set; }

        public bool IsActive { get; set; } = true;
    }
}

using System.ComponentModel.DataAnnotations;

namespace projectcodefrst.Models
{
    public class Kategori
    {
        [Key]
        public int KategoriId { get; set; }

        public string KategoriAdi { get; set; }

        public List<Kitap> Kitaps { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace projectcodefrst.Models
{
    public class Kitap
    {
        [Key]
        public int KitapId { get; set; }

        public string KitapAdi { get; set; }

        public int Sayfa { get; set; }


        public Yazar Yazar { get; set; }
        public int YazarId { get; set; }

        public Kategori Kategori { get; set; }
        public int KategoriID { get; set; }
    }
}

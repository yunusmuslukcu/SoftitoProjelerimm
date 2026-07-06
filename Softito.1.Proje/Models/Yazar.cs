using System.ComponentModel.DataAnnotations;

namespace projectcodefrst.Models
{
    public class Yazar
    {
        [Key]
        public int YazarId { get; set; }

        public string YazarAdi { get; set; }

        public List<Kitap> Kitaps { get; set; }


    }
}

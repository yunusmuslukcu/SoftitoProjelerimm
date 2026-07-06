using System.ComponentModel.DataAnnotations;

namespace projectcodefrst.Models
{
    public class Odunc
    {
        [Key]
        public int OduncId { get; set; }

        public string AlanKisi { get; set; }

        public Kitap Kitap { get; set; }
        public int KitapId { get; set; }

    }
}

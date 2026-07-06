using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Proje5.Models
{
    public class Position
    {

        [Key]
        public int PositionId { get; set; }

        [Required]
        [StringLength(50)]
        public string PositionName { get; set; } 

        [StringLength(10)]
        public string ShortName { get; set; } 

       
    }
}

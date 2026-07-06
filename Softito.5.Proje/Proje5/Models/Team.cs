using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Proje5.Models
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }

        [Required]
        [StringLength(100)]
        public string TeamName { get; set; } 

        [StringLength(50)]
        public string City { get; set; } 


    }
}

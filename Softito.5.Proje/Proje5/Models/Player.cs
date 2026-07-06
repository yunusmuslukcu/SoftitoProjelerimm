using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proje5.Models
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }


        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        public int Age { get; set; }

        public string MarketValue { get; set; }

        public int? TeamId { get; set; }
        public int? PositionId { get; set; }

        [ForeignKey("TeamId")]
        public Team? Team { get; set; }

        [ForeignKey("PositionId")]
        public Position? Position { get; set; }


    }
}

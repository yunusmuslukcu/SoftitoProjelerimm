using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class Branch
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; } 

        [StringLength(300)]
        public string Address { get; set; }

        // İlişki: Bir şubede birden fazla kullanıcı (çalışan) görev alabilir.
        public ICollection<AppUser> Users { get; set; } = new HashSet<AppUser>();
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        // İlişki: Bir kategoride birden fazla ürün olabilir.
        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}

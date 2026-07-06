using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string CompanyName { get; set; }

        [StringLength(50)]
        public string ContactName { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        // İlişki: Bir tedarikçi birden fazla ürün sağlayabilir.
        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}

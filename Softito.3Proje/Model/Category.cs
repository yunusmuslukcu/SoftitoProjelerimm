using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}

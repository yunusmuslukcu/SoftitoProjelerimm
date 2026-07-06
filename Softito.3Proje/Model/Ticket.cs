using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TicketStatus Status { get; set; } = TicketStatus.Open;

        // Kategori İlişkisi (Foreign Key ve Navigation Property)
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        // Kullanıcı İlişkisi (Foreign Key ve Navigation Property)
        public int UserId { get; set; }
        public User User { get; set; }
    }
}

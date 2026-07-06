using System.Collections.Generic;
using Model;

namespace BTProjectt.Models
{
    public class DashboardViewModel
    {
        public int TotalTickets { get; set; }
        public int OpenTickets { get; set; }
        public int InProgressTickets { get; set; }
        public int ResolvedTickets { get; set; }
        public int TotalCategories { get; set; }
        public int TotalUsers { get; set; }

        public List<Ticket> RecentTickets { get; set; } = new List<Ticket>();
        
        // For Chart.js
        public List<string> CategoryNames { get; set; } = new List<string>();
        public List<int> CategoryTicketCounts { get; set; } = new List<int>();
    }
}

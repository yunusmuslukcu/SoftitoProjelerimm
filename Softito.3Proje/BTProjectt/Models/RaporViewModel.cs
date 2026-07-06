using System.Collections.Generic;
using Model;

namespace BTProjectt.Models
{
    public class RaporViewModel
    {
        // 1. COUNT
        public int TotalTickets { get; set; }
        public int TotalCategories { get; set; }
        public int TotalUsers { get; set; }

        // 2. INNER JOIN DTO
        public List<InnerJoinDto> InnerJoinData { get; set; } = new List<InnerJoinDto>();

        // 3. LEFT JOIN DTO
        public List<LeftJoinDto> LeftJoinData { get; set; } = new List<LeftJoinDto>();

        // 4. ORDER BY DTO
        public List<OrderByDto> OrderByData { get; set; } = new List<OrderByDto>();

        // 5. GROUP BY DTO
        public List<GroupByDto> GroupByData { get; set; } = new List<GroupByDto>();
    }

    public class InnerJoinDto
    {
        public string TicketTitle { get; set; }
        public string CategoryName { get; set; }
        public string UserName { get; set; }
        public TicketStatus Status { get; set; }
    }

    public class LeftJoinDto
    {
        public string CategoryName { get; set; }
        public string TicketTitle { get; set; }
    }

    public class OrderByDto
    {
        public int TicketId { get; set; }
        public string TicketTitle { get; set; }
    }

    public class GroupByDto
    {
        public TicketStatus Status { get; set; }
        public int Count { get; set; }
    }
}

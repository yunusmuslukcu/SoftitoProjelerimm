namespace BicyleApi.Models
{
    public class Rental
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int StationId { get; set; }
        public DateTime RentalStartTime { get; set; } 
        public DateTime? RentalEndTime { get; set; } 
        public decimal? TotalPrice { get; set; } 
    }
}

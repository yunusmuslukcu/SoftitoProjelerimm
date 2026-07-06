namespace BicyleApi.Models
{
    public class Station
    {
        public int Id { get; set; }
        public string StationName { get; set; } 
        public string Location { get; set; } 
        public int Capacity { get; set; }
        public int ActiveBicyclesCount { get; set; }
        public bool IsActive { get; set; } 
    }
}

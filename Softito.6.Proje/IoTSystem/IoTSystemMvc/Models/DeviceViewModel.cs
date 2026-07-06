namespace IoTSystemMvc.Models
{
    public class DeviceViewModel
    {
        public int Id { get; set; }
        public string DeviceName { get; set; } 
        public string IpAddress { get; set; }
        public string MacAddress { get; set; } 
        public bool IsActive { get; set; }
        public string Location { get; set; } 
    }
}

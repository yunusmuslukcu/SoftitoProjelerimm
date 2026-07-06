namespace IoTSystemMvc.Models
{
    public class SystemLogViewModel
    {
        public int Id { get; set; }
        public string LogCode { get; set; } 
        public string LogLevel { get; set; } 
        public string Description { get; set; } 
        public string FixSuggestion { get; set; } 
        public DateTime LogDate { get; set; }
    }
}

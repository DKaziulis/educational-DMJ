using System.ComponentModel.DataAnnotations;

namespace Student_Planner.Models
{
    public class Event
    {
        public uint EventID { get; set; }
        public string EventName { get; set; } = string.Empty;
        public string EventDescription { get; set; } = string.Empty;
        public DateTime EventStartTime { get; set; }
        public DateTime EventEndTime { get; set; }
        public TimeSpan EventDuration { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}

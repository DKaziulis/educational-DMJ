using Student_Planner.Controllers;
using System.ComponentModel.DataAnnotations;

namespace Student_Planner.Models
{
    public class Event
    {
        public int Id { get; set; }
        [MaxLength(60)]
        public string? Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EventEndTime { get; set; }
        public string? Description { get; set; }
        public TimeSpan EventDuration { get; set; }
        public EventsController.CourseGroup CourseGroup { get; set; }

    }
}

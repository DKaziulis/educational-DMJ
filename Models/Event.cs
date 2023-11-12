using Student_Planner.Controllers;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Student_Planner.Models
{
    public class Event
    {
        public Event() { }
        public Event(string? name, DateTime beginDate, TimeOnly eventEndTime, string? description, TimeDuration.Time eventDuration)
        {
            Name = name;
            BeginDate = beginDate;
            EndTime = eventEndTime;
            Description = description;
            EventDuration = eventDuration.ToTimeSpan();
        }
        public int Id { get; set; }

        private string? name;
        [MaxLength(60)]
        public string? Name
        {
            get { return name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    name = value;
                }
                else if (Regex.IsMatch(value, @"^[A-Za-z0-9\s-]+$"))
                {
                    name = value;
                }
                else
                {
                    throw new ArgumentException("Invalid name format.");
                }
            }
        }
        public DateTime BeginDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string? Description { get; set; }
        public TimeSpan EventDuration
        {
            get { return CalculateEventDuration(); }
            set { }
        }
        public EventsController.CourseGroup CourseGroup { get; set; }
        private TimeSpan CalculateEventDuration()
        {
            // Calculate event duration based on BeginDate and EventEndTime
            return EndTime - StartTime;
        }
    }
}

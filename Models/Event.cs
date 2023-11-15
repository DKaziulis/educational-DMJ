using Microsoft.EntityFrameworkCore;
using Student_Planner.Controllers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Student_Planner.Services;

namespace Student_Planner.Models
{
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }     //Primary key for the Event entity
        [MaxLength(60)]
        public string? Name { get; set; }
        public DateTime BeginDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string? Description { get; set; }

        public int DayId { get; set; }
        public Day? Day { get; set; }

        public Event() { }
        public Event(string? name, DateTime beginDate, TimeOnly eventEndTime, string? description, TimeDuration.Time eventDuration)
        {
            Name = name;
            BeginDate = beginDate;
            EndTime = eventEndTime;
            Description = description;
            EventDuration = eventDuration.ToTimeSpan();
        }

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

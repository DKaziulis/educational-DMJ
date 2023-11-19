using Student_Planner.Controllers;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Student_Planner.Models.Exceptions;

namespace Student_Planner.Models
{
    public class Event : IComparable<Event>
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
        [MaxLength(60)]
        private string? name;
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
                      throw new CharacterException("Invalid name format.");
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

        private TimeSpan CalculateEventDuration()
        {
            // Calculate event duration based on BeginDate and EventEndTime
            return EndTime - StartTime;
        }
        public int CompareTo(Event? other)
        {
            if (other == null)
            {
                return 1;
            }

            // Compare based on the BeginDate property
            return BeginDate.CompareTo(other.BeginDate);
        }
    }
}

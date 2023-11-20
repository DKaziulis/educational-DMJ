using Microsoft.EntityFrameworkCore;

namespace Student_Planner.Models
{
    public class Day : IComparable<Day>
    {
        public int Id { get; set; } //Primary key for the Day entity
        public DateOnly Date{ get; set; }
        public int NumOfEvents { get; set; }
        public List<Event>? events;
        public Day() { }
        public Day(DateOnly Date, List<Event> events) 
        {
            this.Date = Date;
            this.events = events;
        }
        public int CompareTo(Day? other)
        {
            if (other == null)
            {
                return 1;
            }

            return Date.CompareTo(other.Date);
        }
    }
}

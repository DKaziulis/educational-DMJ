using Microsoft.EntityFrameworkCore;

namespace Student_Planner.Models
{
    public class Day
    {
        public int Id { get; set; } //Primary key for the Day table
        public DateOnly Date{ get; set; }
        public int NumOfEvents { get; set; }
        public List<Event>? events;
        public Day() { }
        public Day(DateOnly Date, List<Event> events) 
        {
            this.Date = Date;
            this.events = events;
        }
    }
}

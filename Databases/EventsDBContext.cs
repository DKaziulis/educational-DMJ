using Microsoft.EntityFrameworkCore;
using Student_Planner.Models;
using System.ComponentModel;

namespace Student_Planner.Databases
{
    public class EventsDBContext : DbContext
    {
        public EventsDBContext(DbContextOptions<EventsDBContext> options) : base(options)
        {

        }
        public DbSet<Event> Events { get; set; }
        public DbSet<Day> Days { get; set; }
    }
}

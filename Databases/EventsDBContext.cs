using Microsoft.EntityFrameworkCore;
using Student_Planner.Models;
using System.ComponentModel;

namespace Student_Planner.Databases
{
    public class EventsDBContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Day> Day { get; set; }
        public EventsDBContext(DbContextOptions<EventsDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Day)
                .WithMany(d => d.events)
                .HasForeignKey(e => e.DayId);
        }
    }
}

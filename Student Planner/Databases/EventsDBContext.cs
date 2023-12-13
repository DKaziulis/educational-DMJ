using Microsoft.EntityFrameworkCore;
using Student_Planner.Models;
using System.ComponentModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Student_Planner.Databases
{
    public class EventsDBContext : IdentityUserContext<IdentityUser>
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Day> Days { get; set; }
        public EventsDBContext(DbContextOptions<EventsDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.Day)
                .WithMany(d => d.events)
                .HasForeignKey(e => e.DayId);

        }
    }
}

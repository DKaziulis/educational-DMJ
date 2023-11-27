using Student_Planner.Models;

namespace Student_Planner.Services.Interfaces
{
    public interface IEventServices
    {
        public void CreateEvent(Event newEvent);
        public void EditEvent(Day existingDay, Event updatedEvent);
        public void DeleteEvent(Event existingEvent, Day existingDay);
    }
}

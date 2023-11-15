using Microsoft.EntityFrameworkCore;
using Student_Planner.Databases;
using Student_Planner.Models;

namespace Student_Planner.Services
{
    public class EventOperator
    {
        public static Event EditEvent(Day? existingDay, Event updatedEvent)
        {
            if (existingDay != null && existingDay.events != null)
            {
                var existingEvent = existingDay.events.FirstOrDefault(e => e.Id == updatedEvent.Id);
                DateOnly tempShortDate = DateOnly.FromDateTime(updatedEvent.BeginDate);

                if (existingEvent != null)
                {
                    // Preserves the original day by setting the event's date to the existing day's date
                    updatedEvent.BeginDate = updatedEvent.BeginDate.Date.Add(existingEvent.BeginDate.TimeOfDay);

                    // Updates event properties
                    existingEvent.Name = updatedEvent.Name;
                    existingEvent.StartTime = updatedEvent.StartTime;
                    existingEvent.CourseGroup = updatedEvent.CourseGroup;
                    existingEvent.Description = updatedEvent.Description;
                }
            }
            return updatedEvent;
        }
        public static Day? DeleteEvent(Day existingDay, Event existingEvent, string filePath)
        {
            JsonHandler<Event> jsonHandler = new JsonHandler<Event>();
            if(existingDay != null && existingDay.events != null)
            {
                existingDay.events.Remove(existingEvent);
                string dayJsonFilePath = Path.Combine(filePath,
                    string.Concat(existingEvent.BeginDate.Date.ToString("yyyy-MM-dd"), ".json"));

                if (existingDay.events.Count == 0)
                {
                    File.Delete(dayJsonFilePath);
                }
                else
                {
                    jsonHandler.SerializeToJson(dayJsonFilePath, existingDay.events);
                }
                return existingDay;
            } else
            {
                return null;
            }
        }
    }
}

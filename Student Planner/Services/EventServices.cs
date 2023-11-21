using Microsoft.EntityFrameworkCore;
using Student_Planner.Models;
using Student_Planner.Repositories.Interfaces;

namespace Student_Planner.Services
{
    public class EventServices
    {
        private readonly IDayRepository _dayRepository;
        private readonly IEventRepository _eventRepository;

        public EventServices(IDayRepository dayRepository, IEventRepository eventRepository)
        {
            _dayRepository = dayRepository;
            _eventRepository = eventRepository;
        }

        public void CreateEvent (Event newEvent)
        {
            //Takes the short date (yyyy-MM-dd) of the passed event, and converts it to DateOnly
            newEvent.StartTime = TimeOnly.FromDateTime(newEvent.BeginDate);
            DateOnly tempShortDate = DateOnly.FromDateTime(newEvent.BeginDate);

            //Checks which day to create the event for
            Day? updatedDay = DayOperator.FindDayForEvent(tempShortDate);

            //If such a date exists
            if (updatedDay != null && updatedDay.events != null)
            {
                updatedDay.events.Add(newEvent);
            }

            //If there is no such date, creates a new one
            else
            {
                updatedDay = new Day
                {
                    Id = Convert.ToInt32(tempShortDate.ToString("yyyyMMdd")),
                    Date = tempShortDate,
                    events = new List<Event> { newEvent }
                };
                _dayRepository.Add(updatedDay);
            }

            newEvent.DayId = updatedDay.Id;

            //Creates a unique ID, which consists of the event Date info, and the serial number of the day's events
            if (updatedDay.events != null)
            {
                string customId = updatedDay.Id.ToString() + updatedDay.events.Count.ToString();
                newEvent.Id = Convert.ToInt32(customId);

                _eventRepository.Add(newEvent);
                _dayRepository.SaveChanges();
            }
            else
            {
                throw new ArgumentException("Error in creating the event");
            }
        }

        public void EditEvent (Day existingDay, Event updatedEvent)
        {
            if (existingDay != null)
            {
                var existingEvent = _eventRepository.GetById(updatedEvent.Id);

                if (existingEvent != null)
                {
                    // Preserves the original day by setting the event's date to the existing day's date
                    existingEvent.BeginDate = updatedEvent.BeginDate.Date.Add(existingEvent.BeginDate.TimeOfDay);

                    // Updates event properties
                    existingEvent.Name = updatedEvent.Name;
                    existingEvent.StartTime = updatedEvent.StartTime;
                    existingEvent.Description = updatedEvent.Description;
                }
                _eventRepository.SaveChanges();
            }
        }
    }
}

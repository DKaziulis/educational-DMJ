using Microsoft.EntityFrameworkCore;
using Student_Planner.Models;
using Student_Planner.Exceptions;
using Microsoft.Extensions.Logging;
using Student_Planner.Repositories.Interfaces;
using System.Text.RegularExpressions;

namespace Student_Planner.Services
{
    public class EventServices
    {
        private readonly IDayRepository _dayRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ILogger<EventServices> _logger;
        private readonly DayOperator _dayOperator;

        public EventServices(IDayRepository dayRepository, IEventRepository eventRepository, ILogger<EventServices> logger,
            DayOperator dayOperator)
        {
            _dayRepository = dayRepository;
            _eventRepository = eventRepository;
            _logger = logger;
            _dayOperator = dayOperator;
        }

        public void CreateEvent (Event newEvent)
        {
            if(!string.IsNullOrEmpty(newEvent.Name))
            {
                try
                {
                    newEvent.Name = newEvent.Name;
                }
                catch (CharacterException ex)
                {
                    _logger.LogError(ex, "Error setting the Name property");
                }

                if (!Regex.IsMatch(newEvent.Name, @"^[A-Za-z0-9\s-]+$"))
                {
                    _logger.LogError("Invalid name format after setting the Name property");
                }
            }
            //Takes the short date (yyyy-MM-dd) of the passed event, and converts it to DateOnly
            newEvent.StartTime = TimeOnly.FromDateTime(newEvent.BeginDate);
            DateOnly tempShortDate = DateOnly.FromDateTime(newEvent.BeginDate);

            //Checks which day to create the event for
            Day? updatedDay = _dayOperator.FindDayForEvent(tempShortDate);

            //If there is no such date, creates a new one
            if (updatedDay == null)
            {
                updatedDay = new Day
                {
                    Id = Convert.ToInt32(tempShortDate.ToString("yyyyMMdd")),
                    Date = tempShortDate,
                    events = new List<Event> { newEvent }
                };
                _dayRepository.Add(updatedDay);
            }

            updatedDay.NumOfEvents++;
            newEvent.DayId = updatedDay.Id;

            //Creates a unique ID, which consists of the event Date info, and the serial number of the day's events
            if (updatedDay != null)
            {
                string customId = updatedDay.Id.ToString() + updatedDay.NumOfEvents.ToString();
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
        public void DeleteEvent(Event existingEvent, Day existingDay)
        {
            int tempId = existingEvent.DayId;
            _eventRepository.Delete(existingEvent);
            existingDay.NumOfEvents--;
            Console.WriteLine(_eventRepository.GetByDayId(tempId));
            //checks if another Event with the current Day's Id exists, if not, deletes the Day
            if (existingDay.NumOfEvents == 0)
            {
                _dayRepository.Delete(existingDay);
            }
            _dayRepository.SaveChanges();
        }
    }
}

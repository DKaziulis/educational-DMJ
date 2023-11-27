using Student_Planner.Models;
using Student_Planner.Repositories.Interfaces;
using Student_Planner.Services.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Student_Planner.Services.Implementations
{
    public class DayOperator : IDayOperator
    {
        private readonly IDayRepository _dayRepository;
        private ConcurrentDictionary<DateOnly, Day> dayDictionary;

        public DayOperator(IDayRepository dayRepository)
        {
            _dayRepository = dayRepository;
            dayDictionary = LoadDays();
        }

        // Loads the list of all existing dates from the repository
        private ConcurrentDictionary<DateOnly, Day> LoadDays()
        {
            var days = _dayRepository.GetAll();
            var dictionary = new ConcurrentDictionary<DateOnly, Day>();

            foreach (var day in days)
            {
                DateOnly date = day.Date;

                dictionary.GetOrAdd(date, _ => day);
            }
            return dictionary;
        }

        // Find a Day by passed date
        public Day? FindDayForEvent(DateOnly shortDate)
        {
            if (dayDictionary.TryGetValue(shortDate, out Day foundDay))
            {
                return foundDay;
            }

            return null;
        }
    }
}
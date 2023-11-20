using Student_Planner.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Student_Planner.Services
{
    public class DayOperator
    {
        // Use ConcurrentDictionary for thread safety
        private static ConcurrentDictionary<DateOnly, Day> dayDictionary = new ConcurrentDictionary<DateOnly, Day>();

        // Loads the list of all existing dates from json files in the specified File Path
        public static ConcurrentDictionary<DateOnly, Day> LoadDays(string filePath)
        {
            // Clear the existing dictionary to reload the days
            dayDictionary.Clear();

            string?[] files = Directory.GetFiles(filePath).Select(Path.GetFileName).ToArray();
            JsonHandler<Event> jsonHandler = new JsonHandler<Event>();

            foreach (string? file in files)
            {
                if (file != null)
                {
                    DateOnly date = DateOnly.Parse(file.Remove(10, 5));

                    // Use GetOrAdd to safely add to the ConcurrentDictionary
                    dayDictionary.GetOrAdd(date, _ =>
                    {
                        Day loadDay = new()
                        {
                            Date = date,
                            events = jsonHandler.DeserializeFromJSON(filePath)
                        };
                        return loadDay;
                    });
                }
            }
            return dayDictionary;
        }

        public static Day? FindDayForEvent(DateOnly shortDate)
        {
            // Use TryGetValue for thread-safe access
            if (dayDictionary.TryGetValue(shortDate, out Day foundDay))
            {
                return foundDay;
            }

            return null;
        }
    }
}
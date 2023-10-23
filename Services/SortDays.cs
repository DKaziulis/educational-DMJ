using Student_Planner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Student_Planner.Services
{
    public static class DayExtensions
    {
        public static List<Day> SortDays(this List<Day> days, string daySortKey = "Date", string eventSortKey = "Name")
        {
            PropertyInfo? property = typeof(Day).GetProperty(daySortKey);
            if (property == null)
            {
                throw new ArgumentException("Invalid or non-existent sorting key.");
            }

            return days
            .OrderBy(d => property.GetValue(d))
            .Select(day => new Day
            {
                Date = day.Date,
                events = day.events?.SortEvents(sortKey: eventSortKey)
            })
            .ToList();
        }
    }
}

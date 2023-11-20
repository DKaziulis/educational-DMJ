using Microsoft.Extensions.Logging;
using Student_Planner.Enums;
using Student_Planner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Student_Planner.Services
{
    public static class DayExtensions
    {
        public static List<Day> SortDays(this List<Day> days, DaySortKey daySortKey = DaySortKey.Date, EventSortKey eventSortKey = EventSortKey.Name)
        {
            PropertyInfo? property = typeof(Day).GetProperty(daySortKey.ToString());
            if (property == null)
            {
                throw new ArgumentException("Invalid or non-existent sorting key.");
            }

            return days
                .OrderBy(d => property.GetValue(d))
                .Select(day => new Day
                {
                    Date = day.Date,
                    NumOfEvents = day.NumOfEvents,
                    events = day.events?.SortEvents(sortKey: eventSortKey)
                })
                .ToList();
        }
    }
}

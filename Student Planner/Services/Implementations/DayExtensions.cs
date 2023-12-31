﻿using Microsoft.Extensions.Logging;
using Student_Planner.Enums;
using Student_Planner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Student_Planner.Services.Implementations
{
    // Define a delegate for sorting
    public delegate IOrderedEnumerable<Day> DaySortDelegate(List<Day> days);

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

        // Example delegate method for sorting by Date
        public static IOrderedEnumerable<Day> SortByDate(List<Day> days)
        {
            return days.OrderBy(d => d.Date);
        }

        // Example delegate method for sorting by NumOfEvents
        public static IOrderedEnumerable<Day> SortByNumOfEvents(List<Day> days)
        {
            return days.OrderBy(d => d.NumOfEvents);
        }
    }
}
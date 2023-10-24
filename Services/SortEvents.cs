using Student_Planner.Enums;
using Student_Planner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Student_Planner.Services
{
    public static class EventExtensions
    {
        public static List<Event> SortEvents(this List<Event> events, EventSortKey sortKey = EventSortKey.Name)
        {
            PropertyInfo? property = typeof(Event).GetProperty(sortKey.ToString());
            if (property == null)
            {
                throw new ArgumentException("Invalid or non-existent sorting key.");
            }

            return events.OrderBy(e => property.GetValue(e)).ToList();
        }
    }
}

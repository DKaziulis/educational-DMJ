using Student_Planner.Enums;
using Student_Planner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Student_Planner.Services.Implementations
{
    public static class EventExtensions
    {
        public static List<T> SortEvents<T>(this List<T> events, EventSortKey sortKey = EventSortKey.Name)
        where T : class, IComparable<T>
        {
            PropertyInfo? property = typeof(T).GetProperty(sortKey.ToString());
            if (property == null)
            {
                throw new ArgumentException("Invalid or non-existent sorting key.");
            }

            return events.OrderBy(e => property.GetValue(e)).ToList();
        }
    }
}

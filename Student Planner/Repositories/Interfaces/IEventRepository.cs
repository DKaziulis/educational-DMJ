using Student_Planner.Models;

namespace Student_Planner.Repositories.Interfaces
{
    public interface IEventRepository : IRepository <Event>
    {
        Event? GetByDayId(int dayId);
        IEnumerable<Event>? GetAllByDayId(int dayId);
    }
}

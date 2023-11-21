using Student_Planner.Models;

namespace Student_Planner.Repositories.Interfaces
{
    public interface IDayRepository : IRepository <Day>
    {
        Day GetByDate (DateOnly date);
    }
}

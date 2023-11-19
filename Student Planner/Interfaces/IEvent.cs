namespace Student_Planner.Interfaces
{
    public interface IEvent
    {
        string Name { get; set; }
        DateTime EventDate { get; set; }
    }
}

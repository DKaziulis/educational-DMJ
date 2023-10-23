

namespace Student_Planner.Models
{
    public class Deadline : IComparable<Deadline>
    {
        public string Title { get; set; }
        public DateTime DueDate { get; set; }

        public Deadline(string title, DateTime dueDate)
        {
            Title = title;
            DueDate = dueDate;
        }

        public int CompareTo(Deadline other)
        {
            if (other == null) return 1;
            return DueDate.CompareTo(other.DueDate);
        }
    }
}
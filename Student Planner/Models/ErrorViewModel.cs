namespace Student_Planner.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        //Lambda Expression
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
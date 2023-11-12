namespace Student_Planner.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        //Lamdba Expression
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
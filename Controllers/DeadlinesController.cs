using Microsoft.AspNetCore.Mvc;
using Student_Planner.Models;

namespace Student_Planner.Controllers;

public class DeadlinesController : Controller
{
    public ActionResult Index()
    {
        var deadlines = new List<Deadline>
        {
            new("Task 1", new DateTime(2023, 10, 25, 23, 59, 59)),
            new("Task 2", new DateTime(2023, 10, 24, 18, 0, 0)),
            new("Task 3", new DateTime(2023, 10, 26, 23, 59, 59))
        };

        deadlines.Sort(); // Sort the deadlines based on due date

        return View(deadlines);
    }

    // Other controller actions (Create, Edit, Delete) can be added here
}
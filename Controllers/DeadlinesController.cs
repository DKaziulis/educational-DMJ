using Microsoft.AspNetCore.Mvc;
using Student_Planner.Models;

namespace Student_Planner.Controllers;

public class DeadlinesController : Controller
{
    private static List<Deadline> _deadlines = new List<Deadline>
    {
        new(1,"PSI lab1", new DateTime(2023, 10, 25, 23, 59, 59),"dont be late!"),
        new(2,"FP task1", new DateTime(2023, 10, 24, 18, 0, 0),"-2 points if late"),
        new(3,"DBVS 3,4", new DateTime(2023, 10, 26, 23, 59, 59),"my email")
    };

    public ActionResult Index()
    {
        

        _deadlines.Sort();

        return View(_deadlines);
    }

    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(Deadline deadline)
    {
        _deadlines.Add(deadline);
        _deadlines.Sort();
        return RedirectToAction("Index");
    }

    public ActionResult Edit(int id)
    {
        var deadline = _deadlines.Find(d => d.Id == id);
        if (deadline == null)
        {
            return NotFound();
        }

        return View(deadline);
    }

    [HttpPost]
    public ActionResult Edit(Deadline deadline)
    {
        var index = _deadlines.FindIndex(d => d.Id == deadline.Id);
        if (index != -1)
        {
            _deadlines[index] = deadline;
            _deadlines.Sort();
        }

        return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
        var deadline = _deadlines.Find(d => d.Id == id);
        if (deadline == null)
        {
            return NotFound();
        }

        _deadlines.Remove(deadline);
        return RedirectToAction("Index");
    }
}
using HumanResourcesDBFirst.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HumanResourcesDBFirst.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext dbContext;

        public DashboardController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            ViewBag.TotalEmployees = dbContext.Employees.Count();

            ViewBag.TotalDepartments = dbContext.Departments.Count();

            ViewBag.PendingLeaves = dbContext.Leaves.Count(x => x.Status == "Pending");
            ViewBag.ActiveLeaves = dbContext.Leaves
                .Where(x => x.Status == "Approved" &&
                x.StartDate <= DateOnly.FromDateTime(DateTime.Now) &&
                x.EndDate >= DateOnly.FromDateTime(DateTime.Now))
                .Count();

            ViewBag.RecentEmployees = dbContext.Employees
                .Include(x => x.Department)
                .Include(x => x.Position)
                .OrderByDescending(x => x.EmployeeId)
                .Take(5)
                .ToList();

            ViewBag.PendingLeavesList = dbContext.Leaves
                .Include(x => x.Employee)
                .Include(x => x.LeaveType)
                .Where(x => x.Status == "Pending")
                .Take(5)
                .ToList();
            return View();
        }
    }
}

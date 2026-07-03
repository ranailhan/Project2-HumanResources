using HumanResourcesDBFirst.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HumanResourcesDBFirst.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly AppDbContext _context;

        public ReportsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // 1. Plain LINQ: Grouping by department name directly using EF Core navigation properties.
            var departmentStats = _context.Employees
                .Where(e => e.IsDeleted != true)
                .GroupBy(e => e.Department!.DepartmentName)
                .Select(g => new DepartmentStatViewModel
                {
                    DepartmentName = g.Key ?? "Belirtilmemiş",
                    EmployeeCount = g.Count(),
                    AverageSalary = g.Average(e => e.Salary) ?? 0,
                    TotalSalary = g.Sum(e => e.Salary) ?? 0
                })
                .ToList();

            // 2. Join LINQ: Explicitly joining Employees, Departments, and Positions tables using join syntax.
            var employeeDetails = (from e in _context.Employees
                                   where e.IsDeleted != true
                                   join d in _context.Departments on e.DepartmentId equals d.DepartmentId into deptGroup
                                   from dept in deptGroup.DefaultIfEmpty()
                                   join p in _context.Positions on e.PositionId equals p.PositionId into posGroup
                                   from pos in posGroup.DefaultIfEmpty()
                                   select new EmployeeDetailReportViewModel
                                   {
                                       EmployeeId = e.EmployeeId,
                                       FullName = e.FirstName + " " + e.LastName,
                                       DepartmentName = dept != null ? dept.DepartmentName : "Belirtilmemiş",
                                       PositionName = pos != null ? pos.PositionName : "Belirtilmemiş",
                                       Salary = e.Salary ?? 0,
                                       HireDate = e.HireDate
                                   })
                                   .ToList();

            var viewModel = new ReportsViewModel
            {
                DepartmentStats = departmentStats,
                EmployeeDetails = employeeDetails
            };

            return View(viewModel);
        }
    }
}

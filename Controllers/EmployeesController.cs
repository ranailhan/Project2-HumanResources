using HumanResourcesDBFirst.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace HumanResourcesDBFirst.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly AppDbContext _context;
        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string search, string searchField)
        {
            var query = _context.Employees
                .Where(x => x.IsDeleted != true)
                .Include(x => x.Department)
                .Include(x => x.Position)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = searchField switch
                {
                    "id" => query.Where(x => x.EmployeeId.ToString().Contains(search)),
                    "name" => query.Where(x => x.FirstName.ToString().Contains(search)),
                    "surname" => query.Where(x => x.LastName.ToString().Contains(search)),
                    "mail" => query.Where(x => x.Email.ToString().Contains(search)),
                    "number" => query.Where(x => x.Phone.ToString().Contains(search)),
                    "hireDate" => query.Where(x => x.HireDate.ToString().Contains(search)),
                    "salary" => query.Where(x => x.Salary.ToString().Contains(search)),
                    "department" => query.Where(x => x.Department.DepartmentName.ToString().Contains(search)),
                    "position" => query.Where(x => x.Position.PositionName.ToString().Contains(search)),
                    _ => query.Where(
                        x => x.EmployeeId.ToString().Contains(search) ||
                        x.FirstName.ToString().Contains(search) ||
                         x.LastName.ToString().Contains(search) ||
                         x.Email.ToString().Contains(search) ||
                         x.Phone.ToString().Contains(search) ||
                         x.HireDate.ToString().Contains(search) ||
                         x.Salary.ToString().Contains(search) ||
                         x.Department.DepartmentName.ToString().Contains(search) ||
                         x.Position.PositionName.ToString().Contains(search)

                    )
                }
            ;
            }
            return View(query.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {

            _context.Employees.Add(employee);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var employee = _context.Employees.Find(id);
            ViewBag.Departments = _context.Departments.ToList();
            ViewBag.Positions = _context.Positions.ToList();
            return View(employee);
        }

        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            _context.Employees.Update(employee);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var employee = _context.Employees.Find(id);
            ViewBag.Departments = _context.Departments.ToList();
            ViewBag.Positions = _context.Positions.ToList();
            return View(employee);
        }

        [HttpPost]
        public IActionResult Delete(Employee employee)
        {
            employee.IsDeleted = true;
            _context.Employees.Update(employee);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

using HumanResourcesDBFirst.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HumanResourcesDBFirst.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly AppDbContext _context;
        //TODO: Employee silinince fkdan patlıyor. Soft delete yapılabilir.
        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var results = _context.Employees
                .Include(x=>x.Department)
                .Include(x=>x.Position)
                .ToList();
            return View(results);
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
            ViewBag.Departments = _context.Departments.ToList();
            ViewBag.Positions = _context.Positions.ToList();
            _context.Employees.Update(employee);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var employee = _context.Employees.Find(id);
            return View(employee);
        }

        [HttpPost]
        public IActionResult Delete(Employee employee)
        {
            _context.Employees.Remove(employee);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

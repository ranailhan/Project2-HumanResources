using HumanResourcesDBFirst.Models;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourcesDBFirst.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly AppDbContext _context;

        public DepartmentsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var results = _context.Departments.ToList();
            return View(results);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Department department)
        {

            _context.Departments.Add(department);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var department = _context.Departments.Find(id);
            return View(department);
        }

        [HttpPost]
        public IActionResult Edit(Department department)
        {
            _context.Departments.Update(department);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var department = _context.Departments.Find(id);
            return View(department);
        }

        [HttpPost]
        public IActionResult Delete(Department department)
        {
            _context.Departments.Remove(department);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

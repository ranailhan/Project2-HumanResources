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

        public IActionResult Index(string search, string searchField)
        {
            var query = _context.Departments.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = searchField switch
                {
                    "id" => query.Where(x => x.DepartmentId.ToString().Contains(search)),
                    "name" => query.Where(x => x.DepartmentName.Contains(search)),
                    "description" => query.Where(x => x.Description.Contains(search)),
                    _ => query.Where(x =>
                        x.DepartmentId.ToString().Contains(search) ||
                        x.DepartmentName.Contains(search) ||
                        x.Description.Contains(search))
                };
            }
            return View(query.ToList());
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

using HumanResourcesDBFirst.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HumanResourcesDBFirst.Controllers
{
    public class LeavesController : Controller
    {
        private readonly AppDbContext _context;

        public LeavesController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var results = _context.Leaves
                .Include(x => x.Employee)
                .Include(x => x.LeaveType)
                .ToList();
            return View(results);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Employees = _context.Employees.ToList();
            ViewBag.LeaveTypes = _context.LeaveTypes.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Leave leave)
        {
            _context.Leaves.Add(leave);
            _context.SaveChanges();
            ViewBag.Employees = _context.Employees.ToList();
            ViewBag.LeaveTypes = _context.LeaveTypes.ToList();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var leaf = _context.Leaves.Find(id);
            ViewBag.Employees = _context.Employees.ToList();
            ViewBag.LeaveTypes = _context.LeaveTypes.ToList();
            return View(leaf);
        }

        [HttpPost]
        public IActionResult Edit(Leave leave)
        {
            _context.Leaves.Update(leave);
            _context.SaveChanges();
            ViewBag.Employees = _context.Employees.ToList();
            ViewBag.LeaveTypes = _context.LeaveTypes.ToList();
            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var leaf = _context.Leaves
                .Include(x => x.Employee)
                .Include(x => x.LeaveType)
                .FirstOrDefault(x => x.LeaveId == id);
            return View(leaf);
        }

        [HttpPost]
        public IActionResult Delete(Leave leave)
        {
            _context.Leaves.Remove(leave);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}


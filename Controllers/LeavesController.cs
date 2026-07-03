using HumanResourcesDBFirst.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace HumanResourcesDBFirst.Controllers
{
    [Authorize]
    public class LeavesController : Controller
    {
        private readonly AppDbContext _context;

        public LeavesController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string search, string searchField)
        {
            var query = _context.Leaves
                .Include(x => x.Employee)
                .Where(x => x.Employee.IsDeleted != true)
                .Include(x => x.LeaveType)
                .AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = searchField switch
                {
                    "id" => query.Where(x => x.LeaveId.ToString().Contains(search)),
                    "name" => query.Where(x => x.Employee.FirstName.ToString().Contains(search) || x.Employee.LastName.ToString().Contains(search)),
                    "leaveType" => query.Where(x => x.LeaveType.LeaveTypeName.ToString().Contains(search)),
                    "startDate" => query.Where(x => x.StartDate.ToString().Contains(search)),
                    "endDate" => query.Where(x => x.EndDate.ToString().Contains(search)),
                    "reason" => query.Where(x => x.Reason.ToString().Contains(search)),
                    "status" => query.Where(x => x.Status.ToString().Contains(search)),
                    _ => query.Where(x => x.LeaveId.ToString().Contains(search) ||
                    x.Employee.FirstName.ToString().Contains(search) ||
                    x.Employee.LastName.ToString().Contains(search) ||
                    x.LeaveType.LeaveTypeName.ToString().Contains(search) ||
                    x.StartDate.ToString().Contains(search) ||
                    x.EndDate.ToString().Contains(search) ||
                    x.Reason.ToString().Contains(search) ||
                    x.Status.ToString().Contains(search))
                };
            }

            return View(query.ToList());

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

        [HttpGet]
        public IActionResult Approve(int id)
        {
            var leave = _context.Leaves.Find(id);
            if (leave != null)
            {
                leave.Status = "Approved";
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Reject(int id)
        {
            var leave = _context.Leaves.Find(id);
            if (leave != null)
            {
                leave.Status = "Rejected";
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}


using HumanResourcesDBFirst.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace HumanResourcesDBFirst.Controllers
{
    [Authorize]
    public class EmployeePanelController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeePanelController(AppDbContext context)
        {
            _context = context;
        }

        private Employee? GetCurrentEmployee()
        {
            if (User.Identity == null || string.IsNullOrEmpty(User.Identity.Name))
                return null;

            return _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .FirstOrDefault(e => e.Email == User.Identity.Name && e.IsDeleted != true);
        }

        public IActionResult Index()
        {
            var employee = GetCurrentEmployee();
            if (employee == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Calculate leave statistics
            var myLeaves = _context.Leaves.Where(l => l.EmployeeId == employee.EmployeeId).ToList();
            ViewBag.TotalLeaves = myLeaves.Count;
            ViewBag.ApprovedLeaves = myLeaves.Count(l => l.Status == "Approved");
            ViewBag.PendingLeaves = myLeaves.Count(l => l.Status == "Pending");
            ViewBag.RejectedLeaves = myLeaves.Count(l => l.Status == "Rejected");

            return View(employee);
        }

        public IActionResult Leaves()
        {
            var employee = GetCurrentEmployee();
            if (employee == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var leaves = _context.Leaves
                .Include(l => l.LeaveType)
                .Where(l => l.EmployeeId == employee.EmployeeId)
                .OrderByDescending(l => l.StartDate)
                .ToList();

            return View(leaves);
        }

        [HttpGet]
        public IActionResult CreateLeave()
        {
            var employee = GetCurrentEmployee();
            if (employee == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.LeaveTypes = _context.LeaveTypes.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult CreateLeave(Leave leave)
        {
            var employee = GetCurrentEmployee();
            if (employee == null)
            {
                return RedirectToAction("Login", "Account");
            }

            leave.EmployeeId = employee.EmployeeId;
            leave.Status = "Pending"; // Always pending initially

            if (leave.StartDate == null || leave.EndDate == null)
            {
                ModelState.AddModelError(string.Empty, "Başlangıç ve bitiş tarihleri zorunludur.");
                ViewBag.LeaveTypes = _context.LeaveTypes.ToList();
                return View(leave);
            }

            if (leave.StartDate > leave.EndDate)
            {
                ModelState.AddModelError(string.Empty, "Başlangıç tarihi bitiş tarihinden sonra olamaz.");
                ViewBag.LeaveTypes = _context.LeaveTypes.ToList();
                return View(leave);
            }

            _context.Leaves.Add(leave);
            _context.SaveChanges();

            return RedirectToAction("Leaves");
        }

        [HttpGet]
        public IActionResult Suggestions()
        {
            var employee = GetCurrentEmployee();
            if (employee == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var suggestions = _context.WishSuggestions
                .Where(s => s.EmployeeId == employee.EmployeeId)
                .OrderByDescending(s => s.SubmittedAt)
                .ToList();

            return View(suggestions);
        }

        [HttpPost]
        public IActionResult Suggestions(string title, string content)
        {
            var employee = GetCurrentEmployee();
            if (employee == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content))
            {
                ModelState.AddModelError(string.Empty, "Başlık ve İçerik alanları doldurulmalıdır.");
                var suggestionsList = _context.WishSuggestions
                    .Where(s => s.EmployeeId == employee.EmployeeId)
                    .OrderByDescending(s => s.SubmittedAt)
                    .ToList();
                return View(suggestionsList);
            }

            var wish = new WishSuggestion
            {
                EmployeeId = employee.EmployeeId,
                Title = title,
                Content = content,
                SubmittedAt = DateTime.Now
            };

            _context.WishSuggestions.Add(wish);
            _context.SaveChanges();

            return RedirectToAction("Suggestions");
        }
    }
}

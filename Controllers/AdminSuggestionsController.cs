using HumanResourcesDBFirst.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HumanResourcesDBFirst.Controllers
{
    [Authorize]
    public class AdminSuggestionsController : Controller
    {
        private readonly AppDbContext _context;

        public AdminSuggestionsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string search)
        {
            var query = _context.WishSuggestions
                .Include(s => s.Employee)
                .ThenInclude(e => e!.Department)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => 
                    s.Title.Contains(search) || 
                    s.Content.Contains(search) ||
                    (s.Employee != null && (s.Employee.FirstName + " " + s.Employee.LastName).Contains(search))
                );
            }

            var list = query.OrderByDescending(s => s.SubmittedAt).ToList();
            return View(list);
        }

        [HttpGet]
        public IActionResult Reply(int id)
        {
            var suggestion = _context.WishSuggestions
                .Include(s => s.Employee)
                .FirstOrDefault(s => s.Id == id);
            if (suggestion == null)
            {
                return NotFound();
            }
            return View(suggestion);
        }

        [HttpPost]
        public IActionResult Reply(int id, string adminReply)
        {
            var suggestion = _context.WishSuggestions.Find(id);
            if (suggestion == null)
            {
                return NotFound();
            }

            suggestion.AdminReply = adminReply;
            suggestion.Status = "Cevaplandı";
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}

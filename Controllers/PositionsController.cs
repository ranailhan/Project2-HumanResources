using HumanResourcesDBFirst.Models;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourcesDBFirst.Controllers
{
    public class PositionsController : Controller
    {
        private readonly AppDbContext _context;

        public PositionsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var results = _context.Positions.ToList();
            return View(results);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Position position)
        {
            _context.Positions.Add(position);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var position = _context.Positions.Find(id);
            return View(position);
        }

        [HttpPost]
        public IActionResult Edit(Position position)
        {
            _context.Positions.Update(position);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var position = _context.Positions.Find(id);
            return View(position);
        }

        [HttpPost]
        public IActionResult Delete(Position position)
        {
            _context.Positions.Remove(position);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}


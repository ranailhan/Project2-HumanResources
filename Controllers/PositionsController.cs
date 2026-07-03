using HumanResourcesDBFirst.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HumanResourcesDBFirst.Controllers
{
    [Authorize]
    public class PositionsController : Controller
    {
        private readonly AppDbContext _context;

        public PositionsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string search, string searchField)
        {
            var query = _context.Positions.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = searchField switch
                {
                    "id" => query.Where(x => x.PositionId.ToString().Contains(search)),
                    "name" => query.Where(x => x.PositionName.ToString().Contains(search)),
                    "plevel" => query.Where(x => x.PositionLevel.ToString().Contains(search)),
                    _ => query.Where(x => x.PositionId.ToString().Contains(search) ||
                    x.PositionName.ToString().Contains(search) ||
                    x.PositionLevel.ToString().Contains(search))
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


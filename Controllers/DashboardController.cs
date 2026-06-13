using Microsoft.AspNetCore.Mvc;

namespace HumanResourcesDBFirst.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

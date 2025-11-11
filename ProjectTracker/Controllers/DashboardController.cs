using Microsoft.AspNetCore.Mvc;

namespace ProjectTracker.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

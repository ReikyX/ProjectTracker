using Microsoft.AspNetCore.Mvc;

namespace ProjectTracker.Controllers
{
    public class ProjectController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

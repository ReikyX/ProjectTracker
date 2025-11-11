using Microsoft.AspNetCore.Mvc;

namespace ProjectTracker.Controllers
{
    public class ProjectTaskController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

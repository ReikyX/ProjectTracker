using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectTracker.Data;
using ProjectTracker.Models;
using System.Security.Claims;

namespace ProjectTracker.Controllers
{
    public class ProjectTaskController : Controller
    {
        private readonly AppDbContext _context;

        public ProjectTaskController(AppDbContext context)
        {
            _context = context;
        }

        private int? GetUserId()
        {
            var userString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userString, out int id))
                return id;

            return null;
        }

        public async Task<IActionResult> Index()
        {

            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var projectTasks = await _context.ProjectTasks
                .Include(t => t.Project)
                .Where(t => t.Project.UserId == userId)
                .ToListAsync();

            return View(projectTasks);
        }

        [HttpGet]
        public async Task<IActionResult> CreateTask()
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            // Nur Projekte des Users holen
            var projects = await _context.Projects
                .Where(p => p.UserId == userId)
                .ToListAsync();

            // Leeres Task-Objekt + Liste der Projekte einfügen
            var model = new CreateTask
            {
                Task = new ProjectTask(),
                Projects = projects
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> CreateConfirm(CreateTask model)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            if (!ModelState.IsValid)
            {
                model.Projects = await _context.Projects
                    .Where(p => p.UserId == userId)
                    .ToListAsync();

                return View("CreateTask", model);
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == model.Task.ProjectId && p.UserId == userId);

            if (project == null)
                return NotFound();

            _context.ProjectTasks.Add(model.Task);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditTask(int id)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var task = await _context.ProjectTasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == id && t.Project.UserId == userId);

            if (task == null) return NotFound();

            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> EditConfirm(ProjectTask task)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            if (!ModelState.IsValid)
                return View(task);

            var existing = await _context.ProjectTasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == task.Id && t.Project.UserId == userId);

            if (existing == null) return NotFound();

            existing.Title = task.Title;
            existing.Description = task.Description;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var task = await _context.ProjectTasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == id && t.Project.UserId == userId);

            if (task == null) return NotFound();

            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var task = await _context.ProjectTasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == id && t.Project.UserId == userId);

            if (task == null) return NotFound();

            _context.ProjectTasks.Remove(task);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}

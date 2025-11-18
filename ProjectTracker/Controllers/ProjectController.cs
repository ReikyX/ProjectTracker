using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectTracker.Data;
using ProjectTracker.Models;
using System.Security.Claims;

namespace ProjectTracker.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly AppDbContext _context;

        public ProjectController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Aktuell angemeldeten Benutzer auslesen
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdString, out int userId))
            {
                // Optional: Redirect oder leere Liste
                return View(new List<Project>());
            }

            // Nur Projekte des angemeldeten Benutzers laden
            var projects = await _context.Projects
                .Where(p => p.UserId == userId)
                .Include(p => p.ProjectTasks) // optional: Tasks mitladen
                .ToListAsync();

            return View(projects);
        }

        [HttpGet]
        public IActionResult CreateProject()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(Project project)
        {
            if (!ModelState.IsValid) return View(project);

            // Aktuell angemeldeten Benutzer auslesen
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdString, out int userId))
            {
                ModelState.AddModelError("", "Benutzer nicht gefunden.");
                return View(project);
            }

            // Benutzer aus der Datenbank abrufen
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                ModelState.AddModelError("", "Benutzer nicht gefunden.");
                return View(project);
            }

            // Projekt dem Benutzer zuordnen
            project.UserId = user.Id;

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            return View(project);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProject(int id, Project project)
        {
            if (id != project.Id) return BadRequest();
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdString, out int userId))
                return Unauthorized();
            var existing = await _context.Projects
        .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (existing == null) return NotFound();

            existing.Name = project.Name;
            existing.Description = project.Description;
            existing.StartDate = project.StartDate;
            existing.EndDate = project.EndDate;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            return View(project);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdString, out int userId))
                return Unauthorized();

            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (project == null)
                return NotFound();

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}

using System.ComponentModel.DataAnnotations;

namespace ProjectTracker.Models
{
    public class Project
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Foreign key to User
        public int UserId { get; set; }
        public AppUser? User { get; set; } = null!;

        // Navigation Property to ProjectTasks
        public ICollection<ProjectTask> ProjectTasks { get; set; } = new List<ProjectTask>();
    }
}

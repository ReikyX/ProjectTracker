namespace ProjectTracker.Models
{
    public class ProjectTask
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Foreign key to Project
        public int ProjectId { get; set; }
        public Project? Project { get; set; } = null!;

        //Navigation Property to TimeEntries
        public ICollection<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();
    }
}

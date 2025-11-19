namespace ProjectTracker.Models
{
    public class CreateTask
    {

        public ProjectTask Task { get; set; } = new ProjectTask();
        public IEnumerable<Project> Projects { get; set; } = new List<Project>();
    }
}

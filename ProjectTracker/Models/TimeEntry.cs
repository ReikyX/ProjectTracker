namespace ProjectTracker.Models
{
    public class TimeEntry
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        // Foreign key to ProjectTask
        public int ProjectTaskId { get; set; }
        public ProjectTask ProjectTask { get; set; } = null!;

        public TimeSpan? Duration
        {
            get
            {
                if (EndTime.HasValue) return EndTime.Value - StartTime;
                return null;
            }
        }
    }
}

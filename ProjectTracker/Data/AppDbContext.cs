using Microsoft.EntityFrameworkCore;
using ProjectTracker.Data;
using ProjectTracker.Models;

namespace ProjectTracker.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<TimeEntry> TimeEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Beziehungen
            ConfigureRelationships(modelBuilder);

            // Seed-Daten ausgelagert
            Seed.SeedData(modelBuilder);
        }
        private void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            // User → Projects: RESTRICT
            modelBuilder.Entity<Project>()
                .HasOne(p => p.User)
                .WithMany(u => u.Projects)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Project → Tasks: CASCADE
            modelBuilder.Entity<ProjectTask>()
                .HasOne(t => t.Project)
                .WithMany(p => p.ProjectTasks)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Task → TimeEntries: CASCADE
            modelBuilder.Entity<TimeEntry>()
                .HasOne(te => te.ProjectTask)
                .WithMany(t => t.TimeEntries)
                .HasForeignKey(te => te.ProjectTaskId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

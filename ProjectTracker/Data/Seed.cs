using Microsoft.EntityFrameworkCore;
using ProjectTracker.Models;

namespace ProjectTracker.Data
{
    public static class Seed
    {
        public static void SeedData(ModelBuilder modelBuilder)
        {
            SeedUsers(modelBuilder);
            SeedProjects(modelBuilder);
            SeedTasks(modelBuilder);
        }

        private static void SeedUsers(ModelBuilder modelBuilder)
        {
            // WICHTIG: Feste Werte verwenden, keine dynamischen wie DateTime.UtcNow!
            var seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // WICHTIG: Passwort-Hash auch fest, damit Migration deterministisch ist
            var adminPasswordHash = "$2a$11$xQH3V8Z3qX.7K9J8Y0C4ZeMzN7Y8x9LK7mVqF3P0QwI4Z8R3X7Y8e"; // Admin123!
            var testPasswordHash = "$2a$11$yQH3V8Z3qX.7K9J8Y0C4ZeMzN7Y8x9LK7mVqF3P0QwI4Z8R3X7Y8f"; // Test123!

            // Admin-User
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Email = "admin@projektmanager.de",
                    Password = adminPasswordHash,
                    IsAdmin = true,
                    CreatedAt = seedDate
                }
            );

            // Normaler Test-User
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 2,
                    Email = "test@projektmanager.de",
                    Password = testPasswordHash,
                    IsAdmin = false,
                    CreatedAt = seedDate
                }
            );
        }
        

        private static void SeedProjects(ModelBuilder modelBuilder)
        {
            // Demo-Projekt für Test-User
            modelBuilder.Entity<Project>().HasData(
                new Project
                {
                    Id = 1,
                    Name = "Demo Projekt",
                    Description = "Ein Beispielprojekt zum Testen der Anwendung",
                    StartDate = new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc), // FESTER Wert!
                    EndDate = new DateTime(2025, 2, 1, 0, 0, 0, DateTimeKind.Utc), // FESTER Wert!
                    UserId = 2
                }
            );
        }

        private static void SeedTasks(ModelBuilder modelBuilder)
        {
            // Test-Aufgaben für Demo-Projekt
            modelBuilder.Entity<ProjectTask>().HasData(
                new ProjectTask
                {
                    Id = 1,
                    Title = "Erste Aufgabe",
                    Description = "Dies ist eine Testaufgabe für das Demo-Projekt",
                    ProjectId = 1
                },
                new ProjectTask
                {
                    Id = 2,
                    Title = "Zweite Aufgabe",
                    Description = "Eine weitere Aufgabe zum Testen",
                    ProjectId = 1
                }
            );
        }
    }
}
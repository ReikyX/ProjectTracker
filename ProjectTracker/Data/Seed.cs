using Microsoft.EntityFrameworkCore;
using ProjectTracker.Models;

namespace ProjectTracker.Data
{
    public static class Seed
    {
        public static void SeedData(ModelBuilder modelBuilder)
        {
            SeedAdmin(modelBuilder);
        }

        private static void SeedAdmin(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // Generiere Hash für Admin-Passwort
            var adminPasswordHash = "$2a$11$ATKZ24pUwEMy..3OlieHEuSZ8EQQbZ7AOFlmBETSsuIbKE34Ce3e2"; // Klartext-Passwort: Admin123!

            modelBuilder.Entity<AppUser>().HasData(
                new AppUser
                {
                    Id = 1,
                    Email = "admin@projektmanager.de",
                    FirstName = "Admin",
                    LastName = "User",
                    Password = adminPasswordHash,
                    IsAdmin = true,
                    CreatedAt = seedDate
                }
            );
        }
    }
}

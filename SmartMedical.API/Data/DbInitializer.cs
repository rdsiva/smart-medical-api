using Microsoft.EntityFrameworkCore;
using SmartMedical.Infrastructure.Models;
using System;
using System.Linq;

namespace SmartMedical.API.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Add users
            var johnDoe = new User
            {
                Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                Email = "john.doe@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = DateOnly.FromDateTime(new DateTime(1980, 1, 15)), // Fix for CS0029
                PhoneNumber = "555-123-4567",
                EmailVerified = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var janeSmith = new User
            {
                Id = Guid.NewGuid(),
                Email = "jane.smith@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                FirstName = "Jane",
                LastName = "Smith",
                DateOfBirth = DateOnly.FromDateTime(new DateTime(1975, 6, 22)), // Fix for CS0029
                PhoneNumber = "555-987-6543",
                EmailVerified = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Other code remains unchanged
        }
    }
}

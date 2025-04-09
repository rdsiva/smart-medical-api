using Microsoft.EntityFrameworkCore;
using SmartMedical.Core.Entities.Auth;
using SmartMedical.Core.Entities.Users;
using SmartMedical.Infrastructure.Data;
using System;
using System.Linq;

namespace SmartMedical.API.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Check if there are any users
            if (context.Users.Any())
            {
                return; // DB has been seeded
            }

            // Add roles
            var patientRole = new Role
            {
                Id = Guid.NewGuid(),
                Name = "Patient",
                Description = "Regular user of the application",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var caregiverRole = new Role
            {
                Id = Guid.NewGuid(),
                Name = "Caregiver",
                Description = "User who assists patients with healthcare management",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var healthcareProviderRole = new Role
            {
                Id = Guid.NewGuid(),
                Name = "HealthcareProvider",
                Description = "Doctors, nurses, and other medical professionals",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var adminRole = new Role
            {
                Id = Guid.NewGuid(),
                Name = "Administrator",
                Description = "Technical staff who manage the system",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Roles.AddRange(patientRole, caregiverRole, healthcareProviderRole, adminRole);
            context.SaveChanges();

            // Add users
            var johnDoe = new User
            {
                Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), // Using the hardcoded ID from ProfileController
                Email = "john.doe@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1980, 1, 15),
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
                DateOfBirth = new DateTime(1975, 6, 22),
                PhoneNumber = "555-987-6543",
                EmailVerified = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Users.AddRange(johnDoe, janeSmith);
            context.SaveChanges();

            // Assign roles to users
            var johnDoePatientRole = new UserRole
            {
                UserId = johnDoe.Id,
                RoleId = patientRole.Id
            };

            var janeSmithPatientRole = new UserRole
            {
                UserId = janeSmith.Id,
                RoleId = patientRole.Id
            };

            context.UserRoles.AddRange(johnDoePatientRole, janeSmithPatientRole);
            context.SaveChanges();

            // Add profiles
            var johnDoeProfile = new Profile
            {
                UserId = johnDoe.Id,
                FirstName = johnDoe.FirstName,
                LastName = johnDoe.LastName,
                DateOfBirth = johnDoe.DateOfBirth,
                Gender = "Male",
                PhoneNumber = johnDoe.PhoneNumber,
                Email = johnDoe.Email,
                ProfilePhotoUrl = "https://example.com/photos/johndoe.jpg",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var janeSmithProfile = new Profile
            {
                UserId = janeSmith.Id,
                FirstName = janeSmith.FirstName,
                LastName = janeSmith.LastName,
                DateOfBirth = janeSmith.DateOfBirth,
                Gender = "Female",
                PhoneNumber = janeSmith.PhoneNumber,
                Email = janeSmith.Email,
                ProfilePhotoUrl = "https://example.com/photos/janesmith.jpg",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Profiles.AddRange(johnDoeProfile, janeSmithProfile);
            context.SaveChanges();

            // Add addresses
            var johnDoeAddress = new Address
            {
                Id = Guid.NewGuid(),
                UserId = johnDoe.Id,
                Street = "123 Main St",
                City = "Anytown",
                State = "CA",
                PostalCode = "12345",
                Country = "USA",
                IsPrimary = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var janeSmithAddress = new Address
            {
                Id = Guid.NewGuid(),
                UserId = janeSmith.Id,
                Street = "456 Oak Ave",
                City = "Somewhere",
                State = "NY",
                PostalCode = "67890",
                Country = "USA",
                IsPrimary = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Addresses.AddRange(johnDoeAddress, janeSmithAddress);
            context.SaveChanges();

            // Add emergency contacts
            var johnDoeEmergencyContact = new EmergencyContact
            {
                Id = Guid.NewGuid(),
                UserId = johnDoe.Id,
                Name = "Jane Doe",
                Relationship = "Spouse",
                PhoneNumber = "555-987-6543",
                Email = "jane.doe@example.com",
                IsPrimary = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var janeSmithEmergencyContact = new EmergencyContact
            {
                Id = Guid.NewGuid(),
                UserId = janeSmith.Id,
                Name = "John Smith",
                Relationship = "Spouse",
                PhoneNumber = "555-123-4567",
                Email = "john.smith@example.com",
                IsPrimary = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.EmergencyContacts.AddRange(johnDoeEmergencyContact, janeSmithEmergencyContact);
            context.SaveChanges();
        }
    }
}

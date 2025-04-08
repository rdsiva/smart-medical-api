using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using SmartMedical.API.Controllers;
using SmartMedical.Core.Entities.Users;
using SmartMedical.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace SmartMedical.Tests.UnitTests
{
    public class ProfileControllerTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public ProfileControllerTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "SmartMedicalTestDb_" + Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetProfile_ReturnsOkWithProfile()
        {
            // Arrange
            using var context = new ApplicationDbContext(_options);
            
            // Add user and profile
            var userId = Guid.NewGuid();
            var profile = new Profile
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = "Male",
                PhoneNumber = "1234567890",
                Email = "john.doe@example.com",
                BloodType = "A+",
                Height = 180,
                Weight = 75,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.Profiles.Add(profile);
            await context.SaveChangesAsync();

            var controller = new ProfileController(context);
            
            // Mock user identity
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                    }, "mock"))
                }
            };

            // Act
            var result = await controller.GetProfile();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            
            var returnedProfile = Assert.IsType<Profile>(okResult.Value);
            Assert.Equal(profile.Id, returnedProfile.Id);
            Assert.Equal(profile.FirstName, returnedProfile.FirstName);
            Assert.Equal(profile.LastName, returnedProfile.LastName);
        }

        [Fact]
        public async Task UpdateProfile_ValidData_ReturnsOk()
        {
            // Arrange
            using var context = new ApplicationDbContext(_options);
            
            // Add user and profile
            var userId = Guid.NewGuid();
            var profile = new Profile
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = "Male",
                PhoneNumber = "1234567890",
                Email = "john.doe@example.com",
                BloodType = "A+",
                Height = 180,
                Weight = 75,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.Profiles.Add(profile);
            await context.SaveChangesAsync();

            var controller = new ProfileController(context);
            
            // Mock user identity
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                    }, "mock"))
                }
            };

            // Updated profile data
            var updatedProfile = new Profile
            {
                FirstName = "John",
                LastName = "Smith", // Changed last name
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = "Male",
                PhoneNumber = "9876543210", // Changed phone number
                Email = "john.smith@example.com", // Changed email
                BloodType = "B+", // Changed blood type
                Height = 182, // Changed height
                Weight = 78 // Changed weight
            };

            // Act
            var result = await controller.UpdateProfile(updatedProfile);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            
            // Verify profile was updated in the database
            var dbProfile = await context.Profiles.FirstOrDefaultAsync(p => p.UserId == userId);
            Assert.NotNull(dbProfile);
            Assert.Equal(updatedProfile.LastName, dbProfile.LastName);
            Assert.Equal(updatedProfile.PhoneNumber, dbProfile.PhoneNumber);
            Assert.Equal(updatedProfile.Email, dbProfile.Email);
            Assert.Equal(updatedProfile.BloodType, dbProfile.BloodType);
            Assert.Equal(updatedProfile.Height, dbProfile.Height);
            Assert.Equal(updatedProfile.Weight, dbProfile.Weight);
        }

        [Fact]
        public async Task GetAddresses_ReturnsOkWithAddresses()
        {
            // Arrange
            using var context = new ApplicationDbContext(_options);
            
            // Add user and addresses
            var userId = Guid.NewGuid();
            var addresses = new List<Address>
            {
                new Address
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Street = "123 Main St",
                    City = "New York",
                    State = "NY",
                    PostalCode = "10001",
                    Country = "USA",
                    IsPrimary = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Address
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Street = "456 Park Ave",
                    City = "New York",
                    State = "NY",
                    PostalCode = "10002",
                    Country = "USA",
                    IsPrimary = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };
            context.Addresses.AddRange(addresses);
            await context.SaveChangesAsync();

            var controller = new ProfileController(context);
            
            // Mock user identity
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                    }, "mock"))
                }
            };

            // Act
            var result = await controller.GetAddresses();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            
            var returnedAddresses = Assert.IsType<List<Address>>(okResult.Value);
            Assert.Equal(2, returnedAddresses.Count);
            Assert.Contains(returnedAddresses, a => a.Street == "123 Main St");
            Assert.Contains(returnedAddresses, a => a.Street == "456 Park Ave");
        }

        [Fact]
        public async Task GetEmergencyContacts_ReturnsOkWithContacts()
        {
            // Arrange
            using var context = new ApplicationDbContext(_options);
            
            // Add user and emergency contacts
            var userId = Guid.NewGuid();
            var contacts = new List<EmergencyContact>
            {
                new EmergencyContact
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Name = "Jane Doe",
                    Relationship = "Spouse",
                    PhoneNumber = "1234567890",
                    Email = "jane.doe@example.com",
                    IsPrimary = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new EmergencyContact
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Name = "Bob Smith",
                    Relationship = "Friend",
                    PhoneNumber = "9876543210",
                    Email = "bob.smith@example.com",
                    IsPrimary = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };
            context.EmergencyContacts.AddRange(contacts);
            await context.SaveChangesAsync();

            var controller = new ProfileController(context);
            
            // Mock user identity
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                    }, "mock"))
                }
            };

            // Act
            var result = await controller.GetEmergencyContacts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            
            var returnedContacts = Assert.IsType<List<EmergencyContact>>(okResult.Value);
            Assert.Equal(2, returnedContacts.Count);
            Assert.Contains(returnedContacts, c => c.Name == "Jane Doe");
            Assert.Contains(returnedContacts, c => c.Name == "Bob Smith");
        }
    }
}

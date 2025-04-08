using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using SmartMedical.API.Controllers;
using SmartMedical.Core.Entities.Medications;
using SmartMedical.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace SmartMedical.Tests.UnitTests
{
    public class MedicationsControllerTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public MedicationsControllerTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "SmartMedicalTestDb_" + Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetMedications_ReturnsOkWithMedications()
        {
            // Arrange
            using var context = new ApplicationDbContext(_options);
            
            // Add user and medications
            var userId = Guid.NewGuid();
            var medications = new List<Medication>
            {
                new Medication
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Name = "Aspirin",
                    Dosage = "100mg",
                    Frequency = "Once daily",
                    StartDate = DateTime.UtcNow.AddDays(-30),
                    EndDate = DateTime.UtcNow.AddDays(30),
                    Instructions = "Take with food",
                    PrescribedBy = "Dr. Smith",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Medication
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Name = "Ibuprofen",
                    Dosage = "200mg",
                    Frequency = "Twice daily",
                    StartDate = DateTime.UtcNow.AddDays(-15),
                    EndDate = null,
                    Instructions = "Take as needed for pain",
                    PrescribedBy = "Dr. Johnson",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };
            context.Medications.AddRange(medications);
            await context.SaveChangesAsync();

            var controller = new MedicationsController(context);
            
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
            var result = await controller.GetMedications();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            
            var returnedMedications = Assert.IsType<List<Medication>>(okResult.Value);
            Assert.Equal(2, returnedMedications.Count);
            Assert.Contains(returnedMedications, m => m.Name == "Aspirin");
            Assert.Contains(returnedMedications, m => m.Name == "Ibuprofen");
        }

        [Fact]
        public async Task GetMedication_ValidId_ReturnsOkWithMedication()
        {
            // Arrange
            using var context = new ApplicationDbContext(_options);
            
            // Add user and medication
            var userId = Guid.NewGuid();
            var medicationId = Guid.NewGuid();
            var medication = new Medication
            {
                Id = medicationId,
                UserId = userId,
                Name = "Lisinopril",
                Dosage = "10mg",
                Frequency = "Once daily",
                StartDate = DateTime.UtcNow.AddDays(-60),
                EndDate = null,
                Instructions = "Take in the morning",
                PrescribedBy = "Dr. Williams",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.Medications.Add(medication);
            await context.SaveChangesAsync();

            var controller = new MedicationsController(context);
            
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
            var result = await controller.GetMedication(medicationId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            
            var returnedMedication = Assert.IsType<Medication>(okResult.Value);
            Assert.Equal(medicationId, returnedMedication.Id);
            Assert.Equal("Lisinopril", returnedMedication.Name);
            Assert.Equal("10mg", returnedMedication.Dosage);
        }

        [Fact]
        public async Task GetMedication_InvalidId_ReturnsNotFound()
        {
            // Arrange
            using var context = new ApplicationDbContext(_options);
            
            var userId = Guid.NewGuid();
            var controller = new MedicationsController(context);
            
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
            var result = await controller.GetMedication(Guid.NewGuid()); // Non-existent ID

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task AddMedication_ValidData_ReturnsCreatedWithMedication()
        {
            // Arrange
            using var context = new ApplicationDbContext(_options);
            
            var userId = Guid.NewGuid();
            var controller = new MedicationsController(context);
            
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

            var newMedication = new Medication
            {
                Name = "Metformin",
                Dosage = "500mg",
                Frequency = "Twice daily",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(90),
                Instructions = "Take with meals",
                PrescribedBy = "Dr. Brown",
                IsActive = true
            };

            // Act
            var result = await controller.AddMedication(newMedication);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
            
            var returnedMedication = Assert.IsType<Medication>(createdResult.Value);
            Assert.Equal(newMedication.Name, returnedMedication.Name);
            Assert.Equal(newMedication.Dosage, returnedMedication.Dosage);
            Assert.Equal(userId, returnedMedication.UserId);
            
            // Verify medication was added to the database
            var dbMedication = await context.Medications.FirstOrDefaultAsync(m => m.Name == "Metformin" && m.UserId == userId);
            Assert.NotNull(dbMedication);
        }

        [Fact]
        public async Task UpdateMedication_ValidData_ReturnsNoContent()
        {
            // Arrange
            using var context = new ApplicationDbContext(_options);
            
            // Add user and medication
            var userId = Guid.NewGuid();
            var medicationId = Guid.NewGuid();
            var medication = new Medication
            {
                Id = medicationId,
                UserId = userId,
                Name = "Atorvastatin",
                Dosage = "20mg",
                Frequency = "Once daily",
                StartDate = DateTime.UtcNow.AddDays(-90),
                EndDate = null,
                Instructions = "Take at bedtime",
                PrescribedBy = "Dr. Davis",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.Medications.Add(medication);
            await context.SaveChangesAsync();

            var controller = new MedicationsController(context);
            
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

            // Updated medication data
            var updatedMedication = new Medication
            {
                Id = medicationId,
                Name = "Atorvastatin",
                Dosage = "40mg", // Changed dosage
                Frequency = "Once daily",
                StartDate = DateTime.UtcNow.AddDays(-90),
                EndDate = DateTime.UtcNow.AddDays(180), // Added end date
                Instructions = "Take at bedtime with water", // Changed instructions
                PrescribedBy = "Dr. Davis",
                IsActive = true
            };

            // Act
            var result = await controller.UpdateMedication(medicationId, updatedMedication);

            // Assert
            Assert.IsType<NoContentResult>(result);
            
            // Verify medication was updated in the database
            var dbMedication = await context.Medications.FindAsync(medicationId);
            Assert.NotNull(dbMedication);
            Assert.Equal("40mg", dbMedication.Dosage);
            Assert.Equal("Take at bedtime with water", dbMedication.Instructions);
            Assert.NotNull(dbMedication.EndDate);
        }

        [Fact]
        public async Task DeleteMedication_ValidId_ReturnsNoContent()
        {
            // Arrange
            using var context = new ApplicationDbContext(_options);
            
            // Add user and medication
            var userId = Guid.NewGuid();
            var medicationId = Guid.NewGuid();
            var medication = new Medication
            {
                Id = medicationId,
                UserId = userId,
                Name = "Simvastatin",
                Dosage = "10mg",
                Frequency = "Once daily",
                StartDate = DateTime.UtcNow.AddDays(-120),
                EndDate = null,
                Instructions = "Take at bedtime",
                PrescribedBy = "Dr. Wilson",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.Medications.Add(medication);
            await context.SaveChangesAsync();

            var controller = new MedicationsController(context);
            
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
            var result = await controller.DeleteMedication(medicationId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            
            // Verify medication was deleted from the database
            var dbMedication = await context.Medications.FindAsync(medicationId);
            Assert.Null(dbMedication);
        }
    }
}

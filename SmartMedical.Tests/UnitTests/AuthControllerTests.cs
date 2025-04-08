using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using SmartMedical.API.Controllers;
using SmartMedical.Core.Entities.Auth;
using SmartMedical.Infrastructure.Data;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SmartMedical.Tests.UnitTests
{
    public class AuthControllerTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public AuthControllerTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(x => x["Jwt:Key"]).Returns("YourSuperSecretKeyForTestingPurposesOnly12345");
            _mockConfiguration.Setup(x => x["Jwt:Issuer"]).Returns("SmartMedicalAPI");
            _mockConfiguration.Setup(x => x["Jwt:Audience"]).Returns("SmartMedicalClient");
            _mockConfiguration.Setup(x => x["Jwt:DurationInMinutes"]).Returns("60");

            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "SmartMedicalTestDb_" + Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task Register_ValidUser_ReturnsOk()
        {
            // Arrange
            using var context = new ApplicationDbContext(_options);
            var controller = new AuthController(context, _mockConfiguration.Object);
            var registerModel = new RegisterModel
            {
                Email = "test@example.com",
                Password = "Password123!",
                FirstName = "Test",
                LastName = "User",
                DateOfBirth = DateTime.Now.AddYears(-30),
                PhoneNumber = "1234567890"
            };

            // Act
            var result = await controller.Register(registerModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            
            // Verify user was created in the database
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == registerModel.Email);
            Assert.NotNull(user);
            Assert.Equal(registerModel.FirstName, user.FirstName);
            Assert.Equal(registerModel.LastName, user.LastName);
        }

        [Fact]
        public async Task Register_DuplicateEmail_ReturnsBadRequest()
        {
            // Arrange
            using var context = new ApplicationDbContext(_options);
            
            // Add existing user
            var existingUser = new User
            {
                Email = "existing@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                FirstName = "Existing",
                LastName = "User",
                DateOfBirth = DateTime.Now.AddYears(-25),
                PhoneNumber = "9876543210"
            };
            context.Users.Add(existingUser);
            await context.SaveChangesAsync();

            var controller = new AuthController(context, _mockConfiguration.Object);
            var registerModel = new RegisterModel
            {
                Email = "existing@example.com", // Same email as existing user
                Password = "NewPassword123!",
                FirstName = "New",
                LastName = "User",
                DateOfBirth = DateTime.Now.AddYears(-20),
                PhoneNumber = "5555555555"
            };

            // Act
            var result = await controller.Register(registerModel);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkWithToken()
        {
            // Arrange
            using var context = new ApplicationDbContext(_options);
            
            // Add user
            var user = new User
            {
                Email = "login@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                FirstName = "Login",
                LastName = "User",
                DateOfBirth = DateTime.Now.AddYears(-35),
                PhoneNumber = "1122334455"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var controller = new AuthController(context, _mockConfiguration.Object);
            var loginModel = new LoginModel
            {
                Email = "login@example.com",
                Password = "Password123!"
            };

            // Act
            var result = await controller.Login(loginModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            
            dynamic response = okResult.Value;
            Assert.NotNull(response.token);
            Assert.NotNull(response.refreshToken);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            using var context = new ApplicationDbContext(_options);
            
            // Add user
            var user = new User
            {
                Email = "invalid@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("CorrectPassword123!"),
                FirstName = "Invalid",
                LastName = "User",
                DateOfBirth = DateTime.Now.AddYears(-40),
                PhoneNumber = "9988776655"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var controller = new AuthController(context, _mockConfiguration.Object);
            var loginModel = new LoginModel
            {
                Email = "invalid@example.com",
                Password = "WrongPassword123!" // Wrong password
            };

            // Act
            var result = await controller.Login(loginModel);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }
    }
}

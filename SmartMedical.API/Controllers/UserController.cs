using Microsoft.AspNetCore.Mvc;
using SmartMedical.Business.Interfaces;
using SmartMedical.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartMedical.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                var response = new UserResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DateOfBirth = user.DateOfBirth.ToDateTime(TimeOnly.MinValue), // Convert DateOnly to DateTime
                    PhoneNumber = user.PhoneNumber,
                    EmailVerified = user.EmailVerified ?? false, // Fix for CS0266 and CS8629
                    IsActive = user.IsActive ?? false, // Handle nullable IsActive similarly
                    LastLoginAt = user.LastLoginAt,
                    CreatedAt = user.CreatedAt ?? DateTime.MinValue, // Fix for CS0266 and CS8629
                    UpdatedAt = user.UpdatedAt ?? DateTime.MinValue // Fix for CS0266 and CS8629
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving user with ID {id}");
                return BadRequest(new { message = "Failed to retrieve user" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllAsync();
                var response = users.Select(u => new UserResponse
                {
                    Id = u.Id,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    DateOfBirth = u.DateOfBirth.ToDateTime(TimeOnly.MinValue), // Convert DateOnly to DateTime
                    PhoneNumber = u.PhoneNumber,
                    EmailVerified = u.EmailVerified ?? false, // Fix for CS0266 and CS8629
                    IsActive = u.IsActive ?? false, // Handle nullable IsActive similarly
                    LastLoginAt = u.LastLoginAt,
                    CreatedAt = u.CreatedAt ?? DateTime.MinValue, // Fix for CS0266 and CS8629
                    UpdatedAt = u.UpdatedAt ?? DateTime.MinValue // Fix for CS0266 and CS8629
                }).ToList();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all users");
                return BadRequest(new { message = "Failed to retrieve users" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            try
            {
                if (await _userService.ExistsByEmailAsync(request.Email))
                {
                    return BadRequest(new { message = "User with this email already exists" });
                }

                var user = new User
                {
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    DateOfBirth = DateOnly.FromDateTime(request.DateOfBirth), // Convert DateTime to DateOnly
                    PhoneNumber = request.PhoneNumber,
                    EmailVerified = false,
                    IsActive = true
                };

                var createdUser = await _userService.CreateAsync(user, request.Password);

                var response = new UserResponse
                {
                    Id = createdUser.Id,
                    Email = createdUser.Email,
                    FirstName = createdUser.FirstName,
                    LastName = createdUser.LastName,
                    DateOfBirth = createdUser.DateOfBirth.ToDateTime(TimeOnly.MinValue), // Convert DateOnly to DateTime
                    PhoneNumber = createdUser.PhoneNumber,
                    EmailVerified = createdUser.EmailVerified ?? false, // Fix for CS0266 and CS8629
                    IsActive = createdUser.IsActive ?? false, // Handle nullable IsActive similarly
                    LastLoginAt = createdUser.LastLoginAt,
                    CreatedAt = createdUser.CreatedAt ?? DateTime.MinValue, // Fix for CS0266 and CS8629
                    UpdatedAt = createdUser.UpdatedAt ?? DateTime.MinValue // Fix for CS0266 and CS8629
                };

                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return BadRequest(new { message = "Failed to create user" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
        {
            try
            {
                var existingUser = await _userService.GetByIdAsync(id);
                if (existingUser == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                existingUser.FirstName = request.FirstName;
                existingUser.LastName = request.LastName;
                existingUser.PhoneNumber = request.PhoneNumber;
                existingUser.DateOfBirth = DateOnly.FromDateTime(request.DateOfBirth); // Convert DateTime to DateOnly

                var updatedUser = await _userService.UpdateAsync(existingUser);

                var response = new UserResponse
                {
                    Id = updatedUser.Id,
                    Email = updatedUser.Email,
                    FirstName = updatedUser.FirstName,
                    LastName = updatedUser.LastName,
                    DateOfBirth = updatedUser.DateOfBirth.ToDateTime(TimeOnly.MinValue), // Convert DateOnly to DateTime
                    PhoneNumber = updatedUser.PhoneNumber,
                    EmailVerified = updatedUser.EmailVerified ?? false,
                    IsActive = updatedUser.IsActive ?? false,
                    LastLoginAt = updatedUser.LastLoginAt,
                    CreatedAt = updatedUser.CreatedAt ?? DateTime.MinValue,
                    UpdatedAt = updatedUser.UpdatedAt ?? DateTime.MinValue
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating user with ID {id}");
                return BadRequest(new { message = "Failed to update user" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                var result = await _userService.DeleteAsync(id);
                if (!result)
                {
                    return NotFound(new { message = "User not found" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting user with ID {id}");
                return BadRequest(new { message = "Failed to delete user" });
            }
        }
    }

    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public bool EmailVerified { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateUserRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class UpdateUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
    }
}

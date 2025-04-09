using Microsoft.AspNetCore.Mvc;
using SmartMedical.Business.Interfaces;
using SmartMedical.Core.Entities.Users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartMedical.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(IProfileService profileService, ILogger<ProfileController> logger)
        {
            _profileService = profileService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                // For now, we're using a hardcoded user ID for demonstration
                // In a real application, this would come from the authenticated user
                var userId = Guid.Parse("235aa9a3-d82a-4331-9ea1-033807ddd64c");
                
                var profile = await _profileService.GetProfileByUserIdAsync(userId);
                if (profile == null)
                {
                    return NotFound(new { message = "Profile not found" });
                }
                
                var response = new ProfileResponse
                {
                    UserId = profile.UserId,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    DateOfBirth = profile.DateOfBirth,
                    Gender = profile.Gender,
                    PhoneNumber = profile.PhoneNumber,
                    Email = profile.Email,
                    ProfilePhotoUrl = profile.ProfilePhotoUrl
                };
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving profile");
                return BadRequest(new { message = "Failed to retrieve profile" });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            try
            {
                // For now, we're using a hardcoded user ID for demonstration
                // In a real application, this would come from the authenticated user
                var userId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");
                
                var existingProfile = await _profileService.GetProfileByUserIdAsync(userId);
                if (existingProfile == null)
                {
                    return NotFound(new { message = "Profile not found" });
                }
                
                // Update profile properties
                existingProfile.FirstName = request.FirstName;
                existingProfile.LastName = request.LastName;
                existingProfile.DateOfBirth = request.DateOfBirth;
                existingProfile.Gender = request.Gender;
                existingProfile.PhoneNumber = request.PhoneNumber;
                existingProfile.Email = request.Email;
                
                await _profileService.UpdateProfileAsync(userId, existingProfile);
                
                return Ok(new { message = "Profile updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating profile");
                return BadRequest(new { message = "Failed to update profile" });
            }
        }

        [HttpGet("addresses")]
        public async Task<IActionResult> GetAddresses()
        {
            try
            {
                // For now, we're using a hardcoded user ID for demonstration
                // In a real application, this would come from the authenticated user
                var userId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");
                
                var addresses = await _profileService.GetAddressesByUserIdAsync(userId);
                
                var response = addresses.Select(a => new AddressResponse
                {
                    Id = a.Id,
                    Street = a.Street,
                    City = a.City,
                    State = a.State,
                    PostalCode = a.PostalCode,
                    Country = a.Country,
                    IsPrimary = a.IsPrimary
                }).ToList();
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving addresses");
                return BadRequest(new { message = "Failed to retrieve addresses" });
            }
        }

        [HttpGet("emergency-contacts")]
        public async Task<IActionResult> GetEmergencyContacts()
        {
            try
            {
                // For now, we're using a hardcoded user ID for demonstration
                // In a real application, this would come from the authenticated user
                var userId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");
                
                var contacts = await _profileService.GetEmergencyContactsByUserIdAsync(userId);
                
                var response = contacts.Select(c => new EmergencyContactResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    Relationship = c.Relationship,
                    PhoneNumber = c.PhoneNumber,
                    Email = c.Email,
                    IsPrimary = c.IsPrimary
                }).ToList();
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving emergency contacts");
                return BadRequest(new { message = "Failed to retrieve emergency contacts" });
            }
        }
    }

    public class ProfileResponse
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ProfilePhotoUrl { get; set; }
    }

    public class UpdateProfileRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }

    public class AddressResponse
    {
        public Guid Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public bool IsPrimary { get; set; }
    }

    public class EmergencyContactResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Relationship { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsPrimary { get; set; }
    }
}

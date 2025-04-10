using Microsoft.Extensions.Logging;
using SmartMedical.Business.Interfaces;
using SmartMedical.Infrastructure.Models;
using SmartMedical.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartMedical.Business.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly ILogger<ProfileService> _logger;

        public ProfileService(IProfileRepository profileRepository, ILogger<ProfileService> logger)
        {
            _profileRepository = profileRepository;
            _logger = logger;
        }

        public async Task<Profile> GetProfileByUserIdAsync(Guid userId)
        {
            try
            {
                return await _profileRepository.GetByUserIdAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving profile for user ID {userId}");
                throw;
            }
        }

        public async Task<Profile> UpdateProfileAsync(Guid userId, Profile profile)
        {
            try
            {
                if (!await _profileRepository.ExistsAsync(userId))
                {
                    throw new InvalidOperationException($"Profile for user ID {userId} does not exist");
                }

                profile.UserId = userId; // Ensure the correct user ID is set
                profile.UpdatedAt = DateTime.UtcNow;
                return await _profileRepository.UpdateAsync(profile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating profile for user ID {userId}");
                throw;
            }
        }

        // Address related methods
        public async Task<IEnumerable<Address>> GetAddressesByUserIdAsync(Guid userId)
        {
            try
            {
                return await _profileRepository.GetAddressesByUserIdAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving addresses for user ID {userId}");
                throw;
            }
        }

        public async Task<Address> GetAddressByIdAsync(Guid addressId)
        {
            try
            {
                return await _profileRepository.GetAddressByIdAsync(addressId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving address with ID {addressId}");
                throw;
            }
        }

        public async Task<Address> CreateAddressAsync(Address address)
        {
            try
            {
                address.CreatedAt = DateTime.UtcNow;
                address.UpdatedAt = DateTime.UtcNow;
                return await _profileRepository.CreateAddressAsync(address);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating address for user ID {address.UserId}");
                throw;
            }
        }

        public async Task<Address> UpdateAddressAsync(Address address)
        {
            try
            {
                address.UpdatedAt = DateTime.UtcNow;
                return await _profileRepository.UpdateAddressAsync(address);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating address with ID {address.Id}");
                throw;
            }
        }

        public async Task<bool> DeleteAddressAsync(Guid addressId)
        {
            try
            {
                return await _profileRepository.DeleteAddressAsync(addressId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting address with ID {addressId}");
                throw;
            }
        }

        // Emergency contact related methods
        public async Task<IEnumerable<EmergencyContact>> GetEmergencyContactsByUserIdAsync(Guid userId)
        {
            try
            {
                return await _profileRepository.GetEmergencyContactsByUserIdAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving emergency contacts for user ID {userId}");
                throw;
            }
        }

        public async Task<EmergencyContact> GetEmergencyContactByIdAsync(Guid contactId)
        {
            try
            {
                return await _profileRepository.GetEmergencyContactByIdAsync(contactId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving emergency contact with ID {contactId}");
                throw;
            }
        }

        public async Task<EmergencyContact> CreateEmergencyContactAsync(EmergencyContact contact)
        {
            try
            {
                contact.CreatedAt = DateTime.UtcNow;
                contact.UpdatedAt = DateTime.UtcNow;
                return await _profileRepository.CreateEmergencyContactAsync(contact);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating emergency contact for user ID {contact.UserId}");
                throw;
            }
        }

        public async Task<EmergencyContact> UpdateEmergencyContactAsync(EmergencyContact contact)
        {
            try
            {
                contact.UpdatedAt = DateTime.UtcNow;
                return await _profileRepository.UpdateEmergencyContactAsync(contact);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating emergency contact with ID {contact.Id}");
                throw;
            }
        }

        public async Task<bool> DeleteEmergencyContactAsync(Guid contactId)
        {
            try
            {
                return await _profileRepository.DeleteEmergencyContactAsync(contactId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting emergency contact with ID {contactId}");
                throw;
            }
        }
    }
}

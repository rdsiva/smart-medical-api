using Microsoft.Extensions.Logging;
using SmartMedical.Business.Interfaces;
using SmartMedical.Core.Entities.Auth;
using SmartMedical.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartMedical.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            try
            {
                return await _userRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving user with ID {id}");
                throw;
            }
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            try
            {
                return await _userRepository.GetByEmailAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving user with email {email}");
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            try
            {
                return await _userRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all users");
                throw;
            }
        }

        public async Task<User> CreateAsync(User user, string password)
        {
            try
            {
                if (await _userRepository.ExistsByEmailAsync(user.Email))
                {
                    throw new InvalidOperationException($"User with email {user.Email} already exists");
                }

                // In a real implementation, you would hash the password here
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
                user.CreatedAt = DateTime.UtcNow;
                user.UpdatedAt = DateTime.UtcNow;

                return await _userRepository.CreateAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating user with email {user.Email}");
                throw;
            }
        }

        public async Task<User> UpdateAsync(User user)
        {
            try
            {
                if (!await _userRepository.ExistsAsync(user.Id))
                {
                    throw new InvalidOperationException($"User with ID {user.Id} does not exist");
                }

                user.UpdatedAt = DateTime.UtcNow;
                return await _userRepository.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating user with ID {user.Id}");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                return await _userRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting user with ID {id}");
                throw;
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            try
            {
                return await _userRepository.ExistsAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking if user with ID {id} exists");
                throw;
            }
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            try
            {
                return await _userRepository.ExistsByEmailAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking if user with email {email} exists");
                throw;
            }
        }
    }
}

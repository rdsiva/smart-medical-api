using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartMedical.Core.Entities.Users;

namespace SmartMedical.Core.Interfaces
{
    public interface IProfileRepository
    {
        Task<Profile> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<Profile>> GetAllAsync();
        Task<Profile> CreateAsync(Profile profile);
        Task<Profile> UpdateAsync(Profile profile);
        Task<bool> DeleteAsync(Guid userId);
        Task<bool> ExistsAsync(Guid userId);
        
        // Address related methods
        Task<IEnumerable<Address>> GetAddressesByUserIdAsync(Guid userId);
        Task<Address> GetAddressByIdAsync(Guid addressId);
        Task<Address> CreateAddressAsync(Address address);
        Task<Address> UpdateAddressAsync(Address address);
        Task<bool> DeleteAddressAsync(Guid addressId);
        
        // Emergency contact related methods
        Task<IEnumerable<EmergencyContact>> GetEmergencyContactsByUserIdAsync(Guid userId);
        Task<EmergencyContact> GetEmergencyContactByIdAsync(Guid contactId);
        Task<EmergencyContact> CreateEmergencyContactAsync(EmergencyContact contact);
        Task<EmergencyContact> UpdateEmergencyContactAsync(EmergencyContact contact);
        Task<bool> DeleteEmergencyContactAsync(Guid contactId);
    }
}

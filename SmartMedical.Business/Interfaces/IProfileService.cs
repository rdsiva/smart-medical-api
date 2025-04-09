using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartMedical.Core.Entities.Users;

namespace SmartMedical.Business.Interfaces
{
    public interface IProfileService
    {
        Task<Profile> GetProfileByUserIdAsync(Guid userId);
        Task<Profile> UpdateProfileAsync(Guid userId, Profile profile);
        
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

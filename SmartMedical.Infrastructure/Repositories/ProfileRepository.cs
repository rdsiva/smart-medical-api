using Microsoft.EntityFrameworkCore;
using SmartMedical.Core.Interfaces;
using SmartMedical.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMedical.Infrastructure.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly ApplicationDbContext _context;

        public ProfileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Profile> GetByUserIdAsync(Guid userId)
        {
            
            return await _context.Profiles
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task<IEnumerable<Profile>> GetAllAsync()
        {
            return await _context.Profiles.ToListAsync();
        }

        public async Task<Profile> CreateAsync(Profile profile)
        {
            profile.CreatedAt = DateTime.UtcNow;
            profile.UpdatedAt = DateTime.UtcNow;
            
            await _context.Profiles.AddAsync(profile);
            await _context.SaveChangesAsync();
            
            return profile;
        }

        public async Task<Profile> UpdateAsync(Profile profile)
        {
            profile.UpdatedAt = DateTime.UtcNow;
            
            _context.Profiles.Update(profile);
            await _context.SaveChangesAsync();
            
            return profile;
        }

        public async Task<bool> DeleteAsync(Guid userId)
        {
            var profile = await _context.Profiles.FindAsync(userId);
            if (profile == null)
                return false;
                
            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> ExistsAsync(Guid userId)
        {
            return await _context.Profiles.AnyAsync(p => p.UserId == userId);
        }

        // Address related methods
        public async Task<IEnumerable<Address>> GetAddressesByUserIdAsync(Guid userId)
        {
            return await _context.Addresses
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        public async Task<Address> GetAddressByIdAsync(Guid addressId)
        {
            return await _context.Addresses.FindAsync(addressId);
        }

        public async Task<Address> CreateAddressAsync(Address address)
        {
            address.CreatedAt = DateTime.UtcNow;
            address.UpdatedAt = DateTime.UtcNow;
            
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();
            
            return address;
        }

        public async Task<Address> UpdateAddressAsync(Address address)
        {
            address.UpdatedAt = DateTime.UtcNow;
            
            _context.Addresses.Update(address);
            await _context.SaveChangesAsync();
            
            return address;
        }

        public async Task<bool> DeleteAddressAsync(Guid addressId)
        {
            var address = await _context.Addresses.FindAsync(addressId);
            if (address == null)
                return false;
                
            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
            
            return true;
        }

        // Emergency contact related methods
        public async Task<IEnumerable<EmergencyContact>> GetEmergencyContactsByUserIdAsync(Guid userId)
        {
            return await _context.EmergencyContacts
                .Where(ec => ec.UserId == userId)
                .ToListAsync();
        }

        public async Task<EmergencyContact> GetEmergencyContactByIdAsync(Guid contactId)
        {
            return await _context.EmergencyContacts.FindAsync(contactId);
        }

        public async Task<EmergencyContact> CreateEmergencyContactAsync(EmergencyContact contact)
        {
            contact.CreatedAt = DateTime.UtcNow;
            contact.UpdatedAt = DateTime.UtcNow;
            
            await _context.EmergencyContacts.AddAsync(contact);
            await _context.SaveChangesAsync();
            
            return contact;
        }

        public async Task<EmergencyContact> UpdateEmergencyContactAsync(EmergencyContact contact)
        {
            contact.UpdatedAt = DateTime.UtcNow;
            
            _context.EmergencyContacts.Update(contact);
            await _context.SaveChangesAsync();
            
            return contact;
        }

        public async Task<bool> DeleteEmergencyContactAsync(Guid contactId)
        {
            var contact = await _context.EmergencyContacts.FindAsync(contactId);
            if (contact == null)
                return false;
                
            _context.EmergencyContacts.Remove(contact);
            await _context.SaveChangesAsync();
            
            return true;
        }
    }
}

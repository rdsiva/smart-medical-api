using System;
using System.Collections.Generic;
using SmartMedical.Core.Entities.AIAssistant;
using SmartMedical.Core.Entities.Appointments;
using SmartMedical.Core.Entities.HealthRecords;
using SmartMedical.Core.Entities.Medications;
using SmartMedical.Core.Entities.Users;

namespace SmartMedical.Core.Entities.Auth
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public bool EmailVerified { get; set; }
        public bool IsActive { get; set; }
        public int FailedLoginAttempts { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<EmergencyContact> EmergencyContacts { get; set; }
        public ICollection<UserProvider> UserProviders { get; set; }
        public ICollection<Condition> Conditions { get; set; }
        public ICollection<Allergy> Allergies { get; set; }
        public ICollection<VitalStat> VitalStats { get; set; }
        public ICollection<Medication> Medications { get; set; }
        public ICollection<Immunization> Immunizations { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<Conversation> Conversations { get; set; }
    }
}

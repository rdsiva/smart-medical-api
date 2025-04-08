using System;
using System.Collections.Generic;

namespace SmartMedical.Core.Entities.Users
{
    public class HealthcareProvider
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Specialty { get; set; }
        public string Facility { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public ICollection<UserProvider> UserProviders { get; set; }
    }
}

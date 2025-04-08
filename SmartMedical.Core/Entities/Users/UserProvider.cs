using System;

namespace SmartMedical.Core.Entities.Users
{
    public class UserProvider
    {
        public Guid UserId { get; set; }
        public Guid ProviderId { get; set; }
        public DateTime? RelationshipStartDate { get; set; }
        public DateTime? LastVisitDate { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public Auth.User User { get; set; }
        public HealthcareProvider Provider { get; set; }
    }
}

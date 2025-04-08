using System;

namespace SmartMedical.Core.Entities.HealthRecords
{
    public class Allergy
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Allergen { get; set; }
        public string Reaction { get; set; }
        public string Severity { get; set; } // 'mild', 'moderate', 'severe'
        public DateTime? DiagnosedDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public Auth.User User { get; set; }
    }
}

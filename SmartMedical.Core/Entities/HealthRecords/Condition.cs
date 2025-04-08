using System;

namespace SmartMedical.Core.Entities.HealthRecords
{
    public class Condition
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public DateTime? OnsetDate { get; set; }
        public string Status { get; set; } // 'active', 'resolved', 'in_remission'
        public string Notes { get; set; }
        public string Severity { get; set; } // 'mild', 'moderate', 'severe'
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public Auth.User User { get; set; }
    }
}

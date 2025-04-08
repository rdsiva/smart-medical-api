using System;

namespace SmartMedical.Core.Entities.HealthRecords
{
    public class VitalStat
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Type { get; set; } // 'weight', 'height', 'blood_pressure', etc.
        public decimal Value { get; set; }
        public string Unit { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public Auth.User User { get; set; }
    }
}

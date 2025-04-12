using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartMedical.Core.Entities.HealthRecords
{
    public class VitalSign
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Type { get; set; } // blood_pressure, heart_rate, respiratory_rate, temperature, weight, height, blood_glucose, oxygen_saturation
        public decimal Value { get; set; }
        public string Unit { get; set; }
        public decimal? SecondaryValue { get; set; } // For blood pressure (diastolic)
        public string SecondaryUnit { get; set; }
        public DateTime MeasurementTime { get; set; }
        public string Source { get; set; } // self-reported, provider-reported, device, imported
        public string DeviceId { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public Auth.User User { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartMedical.Core.Entities.HealthRecords
{
    public class MedicalCondition
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Status { get; set; } // active, resolved, inactive
        public DateTime? DiagnosedDate { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public string Severity { get; set; } // mild, moderate, severe
        public string Notes { get; set; }
        public Guid? DiagnosedById { get; set; }
        public string Source { get; set; } // self-reported, provider-reported, imported
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public Auth.User User { get; set; }
    }
}

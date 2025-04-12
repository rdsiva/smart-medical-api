using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartMedical.Core.Entities.HealthRecords
{
    public class FamilyHistory
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Condition { get; set; }
        public string Relationship { get; set; } // mother, father, sibling, etc.
        public string Status { get; set; } // current, deceased, etc.
        public string AgeAtDiagnosis { get; set; }
        public string AgeAtDeath { get; set; }
        public string Notes { get; set; }
        public string Source { get; set; } // self-reported, provider-reported, imported
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public Auth.User User { get; set; }
    }
}

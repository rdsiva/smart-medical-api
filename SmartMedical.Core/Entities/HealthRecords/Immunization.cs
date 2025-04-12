using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartMedical.Core.Entities.HealthRecords
{
    public class Immunization
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string VaccineCode { get; set; }
        public DateTime AdministrationDate { get; set; }
        public string Manufacturer { get; set; }
        public string LotNumber { get; set; }
        public string AdministeredBy { get; set; }
        public string AdministrationSite { get; set; } // left arm, right arm, etc.
        public string AdministrationRoute { get; set; } // intramuscular, subcutaneous, etc.
        public decimal? DoseQuantity { get; set; }
        public string DoseUnit { get; set; }
        public int? DoseNumber { get; set; }
        public int? TotalDoses { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string Notes { get; set; }
        public string Source { get; set; } // self-reported, provider-reported, imported
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public Auth.User User { get; set; }
    }
}

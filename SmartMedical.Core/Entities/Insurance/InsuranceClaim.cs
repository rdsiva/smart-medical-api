using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartMedical.Core.Entities.Insurance
{
    public class InsuranceClaim
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid InsurancePlanId { get; set; }
        public string ClaimNumber { get; set; }
        public DateTime ServiceDate { get; set; }
        public string ServiceType { get; set; } // office_visit, specialist_visit, urgent_care, emergency, lab_work, imaging, procedure, other
        public string ProviderName { get; set; }
        public string ProviderNpi { get; set; }
        public string DiagnosisCodes { get; set; } // Comma-separated ICD-10 codes
        public string ProcedureCodes { get; set; } // Comma-separated CPT/HCPCS codes
        public decimal BilledAmount { get; set; }
        public decimal? AllowedAmount { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? PatientResponsibility { get; set; }
        public string Status { get; set; } // submitted, in_process, denied, partially_paid, paid, appealed
        public DateTime? SubmissionDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public string DenialReason { get; set; }
        public string Notes { get; set; }
        public string DocumentUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public Auth.User User { get; set; }
        public InsurancePlan InsurancePlan { get; set; }
    }
}

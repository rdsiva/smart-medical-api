using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartMedical.Core.Entities.Insurance
{
    public class InsuranceCoverage
    {
        [Key]
        public Guid Id { get; set; }
        public Guid InsurancePlanId { get; set; }
        public string ServiceType { get; set; } // office_visit, specialist_visit, urgent_care, emergency, lab_work, imaging, procedure, prescription, other
        public decimal? CopayAmount { get; set; }
        public decimal? CoinsurancePercentage { get; set; }
        public decimal? DeductibleAmount { get; set; }
        public decimal? OutOfPocketMaximum { get; set; }
        public bool RequiresPreauthorization { get; set; }
        public bool RequiresReferral { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public InsurancePlan InsurancePlan { get; set; }
    }
}

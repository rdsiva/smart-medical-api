using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartMedical.Core.Entities.Insurance
{
    public class InsurancePlan
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Provider { get; set; }
        public string PlanName { get; set; }
        public string PlanType { get; set; } // hmo, ppo, epo, pos, hdhp, medicare, medicaid, other
        public string MemberId { get; set; }
        public string GroupNumber { get; set; }
        public string SubscriberName { get; set; }
        public string SubscriberRelationship { get; set; } // self, spouse, parent, other
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool IsPrimary { get; set; }
        public string CardImageFront { get; set; }
        public string CardImageBack { get; set; }
        public string CustomerServicePhone { get; set; }
        public string ProviderPhone { get; set; }
        public string ClaimsAddressLine1 { get; set; }
        public string ClaimsAddressLine2 { get; set; }
        public string ClaimsAddressCity { get; set; }
        public string ClaimsAddressState { get; set; }
        public string ClaimsAddressZip { get; set; }
        public string ClaimsAddressCountry { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public Auth.User User { get; set; }
        public ICollection<InsuranceCoverage> CoverageDetails { get; set; }
        public ICollection<InsuranceClaim> Claims { get; set; }
    }
}

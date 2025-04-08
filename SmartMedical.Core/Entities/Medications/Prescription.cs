using System;

namespace SmartMedical.Core.Entities.Medications
{
    public class Prescription
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string MedicationName { get; set; }
        public string PrescribedBy { get; set; }
        public DateTime PrescribedDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int RefillsTotal { get; set; }
        public int RefillsRemaining { get; set; }
        public Guid? PharmacyId { get; set; }
        public string PrescriptionNumber { get; set; }
        public string Status { get; set; } // 'active', 'expired', 'completed'
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public Auth.User User { get; set; }
        public Medication Medication { get; set; }
        public Guid MedicationId { get; set; }
    }
}

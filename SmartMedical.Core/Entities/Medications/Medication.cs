using System;
using System.Collections.Generic;

namespace SmartMedical.Core.Entities.Medications
{
    public class Medication
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public string Instructions { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid? PrescriptionId { get; set; }
        public string PrescribedBy { get; set; }
        public string Reason { get; set; }
        public string MedicationImageUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public Auth.User User { get; set; }
        public ICollection<MedicationSchedule> MedicationSchedules { get; set; }
        public ICollection<MedicationDose> MedicationDoses { get; set; }
    }
}

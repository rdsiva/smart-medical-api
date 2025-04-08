using System;

namespace SmartMedical.Core.Entities.Medications
{
    public class MedicationSchedule
    {
        public Guid Id { get; set; }
        public Guid MedicationId { get; set; }
        public TimeOnly TimeOfDay { get; set; }
        public string Dosage { get; set; }
        public bool WithFood { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public Medication Medication { get; set; }
        public ICollection<MedicationDose> MedicationDoses { get; set; }

    }
}

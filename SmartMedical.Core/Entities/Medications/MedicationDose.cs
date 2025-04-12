using System;
using SmartMedical.Core.Entities.HealthRecords;

namespace SmartMedical.Core.Entities.Medications
{
    public class MedicationDose
    {
        public Guid Id { get; set; }
        public Guid MedicationId { get; set; }
        public DateTime ScheduledTime { get; set; }
        public DateTime? TakenTime { get; set; }
        public string Status { get; set; } // 'scheduled', 'taken', 'missed', 'skipped'
        public string DosageTaken { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public Medication Medication { get; set; }
        public MedicationSchedule Schedule{ get; set; }
        public Guid ScheduleId { get; set; }
    }
}

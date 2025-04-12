using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartMedical.Core.Entities.Medications;

namespace SmartMedical.Core.Interfaces
{
    public interface IMedicationRepository
    {
        Task<IEnumerable<Medication>> GetMedicationsAsync(Guid userId, bool? active = null, string search = null);
        Task<Medication> GetMedicationByIdAsync(Guid id, Guid userId);
        Task<Medication> CreateMedicationAsync(Medication medication);
        Task<Medication> UpdateMedicationAsync(Medication medication);
        Task<bool> DeleteMedicationAsync(Guid id, Guid userId);
        
        // Medication Schedule
        Task<IEnumerable<MedicationSchedule>> GetMedicationSchedulesAsync(Guid medicationId);
        Task<MedicationSchedule> GetMedicationScheduleByIdAsync(Guid id);
        Task<MedicationSchedule> CreateMedicationScheduleAsync(MedicationSchedule schedule);
        Task<MedicationSchedule> UpdateMedicationScheduleAsync(MedicationSchedule schedule);
        Task<bool> DeleteMedicationScheduleAsync(Guid id);
        
        // Medication Doses
        Task<IEnumerable<MedicationDose>> GetMedicationDosesAsync(Guid scheduleId);
        Task<MedicationDose> CreateMedicationDoseAsync(MedicationDose dose);
        Task<MedicationDose> UpdateMedicationDoseAsync(MedicationDose dose);
        Task<bool> DeleteMedicationDoseAsync(Guid id);
        
        // Additional methods for medication management
        Task<IEnumerable<Medication>> GetMedicationsDueForRefillAsync(Guid userId, int daysThreshold);
    }
}

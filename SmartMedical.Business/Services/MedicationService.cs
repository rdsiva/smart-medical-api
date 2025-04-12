using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartMedical.Business.Interfaces;
using SmartMedical.Core.Entities.Medications;
using SmartMedical.Core.Interfaces;

namespace SmartMedical.Business.Services
{
    public class MedicationService : IMedicationService
    {
        private readonly IMedicationRepository _medicationRepository;

        public MedicationService(IMedicationRepository medicationRepository)
        {
            _medicationRepository = medicationRepository;
        }

        public async Task<IEnumerable<Medication>> GetMedicationsAsync(Guid userId, bool? active = null, string search = null)
        {
            return await _medicationRepository.GetMedicationsAsync(userId, active, search);
        }

        public async Task<Medication> GetMedicationByIdAsync(Guid id, Guid userId)
        {
            return await _medicationRepository.GetMedicationByIdAsync(id, userId);
        }

        public async Task<Medication> CreateMedicationAsync(Medication medication)
        {
            // Set default values if not provided
            if (medication.StartDate == default)
            {
                medication.StartDate = DateTime.UtcNow.Date;
            }
            
            if (!medication.IsActive)
            {
                medication.IsActive = true;
            }

            return await _medicationRepository.CreateMedicationAsync(medication);
        }

        public async Task<Medication> UpdateMedicationAsync(Medication medication)
        {
            var existingMedication = await _medicationRepository.GetMedicationByIdAsync(medication.Id, medication.UserId);
            if (existingMedication == null)
            {
                throw new KeyNotFoundException($"Medication with ID {medication.Id} not found");
            }

            return await _medicationRepository.UpdateMedicationAsync(medication);
        }

        public async Task<bool> DeleteMedicationAsync(Guid id, Guid userId)
        {
            return await _medicationRepository.DeleteMedicationAsync(id, userId);
        }

        public async Task<IEnumerable<MedicationSchedule>> GetMedicationSchedulesAsync(Guid medicationId)
        {
            return await _medicationRepository.GetMedicationSchedulesAsync(medicationId);
        }

        public async Task<MedicationSchedule> GetMedicationScheduleByIdAsync(Guid id)
        {
            return await _medicationRepository.GetMedicationScheduleByIdAsync(id);
        }

        public async Task<MedicationSchedule> CreateMedicationScheduleAsync(MedicationSchedule schedule)
        {
            return await _medicationRepository.CreateMedicationScheduleAsync(schedule);
        }

        public async Task<MedicationSchedule> UpdateMedicationScheduleAsync(MedicationSchedule schedule)
        {
            var existingSchedule = await _medicationRepository.GetMedicationScheduleByIdAsync(schedule.Id);
            if (existingSchedule == null)
            {
                throw new KeyNotFoundException($"Medication schedule with ID {schedule.Id} not found");
            }

            return await _medicationRepository.UpdateMedicationScheduleAsync(schedule);
        }

        public async Task<bool> DeleteMedicationScheduleAsync(Guid id)
        {
            return await _medicationRepository.DeleteMedicationScheduleAsync(id);
        }

        public async Task<IEnumerable<MedicationDose>> GetMedicationDosesAsync(Guid scheduleId)
        {
            return await _medicationRepository.GetMedicationDosesAsync(scheduleId);
        }

        public async Task<MedicationDose> CreateMedicationDoseAsync(MedicationDose dose)
        {
            return await _medicationRepository.CreateMedicationDoseAsync(dose);
        }

        public async Task<MedicationDose> UpdateMedicationDoseAsync(MedicationDose dose)
        {
            return await _medicationRepository.UpdateMedicationDoseAsync(dose);
        }

        public async Task<bool> DeleteMedicationDoseAsync(Guid id)
        {
            return await _medicationRepository.DeleteMedicationDoseAsync(id);
        }

        public async Task<IEnumerable<Medication>> GetMedicationsDueForRefillAsync(Guid userId, int daysThreshold)
        {
            return await _medicationRepository.GetMedicationsDueForRefillAsync(userId, daysThreshold);
        }

        public async Task<bool> RecordMedicationDoseAsync(Guid scheduleId, DateTime takenAt, bool taken)
        {
            var schedule = await _medicationRepository.GetMedicationScheduleByIdAsync(scheduleId);
            if (schedule == null)
            {
                return false;
            }

            var dose = new MedicationDose
            {
                ScheduleId = scheduleId,
                ScheduledTime = takenAt,
                ActualTime = takenAt,
                Taken = taken,
                Notes = taken ? "Medication taken as scheduled" : "Medication not taken"
            };

            await _medicationRepository.CreateMedicationDoseAsync(dose);
            return true;
        }
    }
}

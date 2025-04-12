using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartMedical.Core.Entities.Medications;
using SmartMedical.Core.Interfaces;
using SmartMedical.Infrastructure.Data;

namespace SmartMedical.Infrastructure.Repositories
{
    public class MedicationRepository : IMedicationRepository
    {
        private readonly ApplicationDbContext _context;

        public MedicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Medication>> GetMedicationsAsync(Guid userId, bool? active = null, string search = null)
        {
            var query = _context.Medications
                .Where(m => m.UserId == userId);

            if (active.HasValue)
            {
                query = query.Where(m => m.IsActive == active.Value);
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(m => m.Name.Contains(search) || 
                                         m.Reason.Contains(search) || 
                                         m.PrescribedBy.Contains(search));
            }

            return await query.OrderBy(m => m.Name).ToListAsync();
        }

        public async Task<Medication> GetMedicationByIdAsync(Guid id, Guid userId)
        {
            return await _context.Medications
                .Include(m => m.MedicationSchedules)
                .Include(m => m.Prescriptions)
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);
        }

        public async Task<Medication> CreateMedicationAsync(Medication medication)
        {
            medication.Id = Guid.NewGuid();
            medication.CreatedAt = DateTime.UtcNow;
            medication.UpdatedAt = DateTime.UtcNow;
            
            _context.Medications.Add(medication);
            await _context.SaveChangesAsync();
            
            return medication;
        }

        public async Task<Medication> UpdateMedicationAsync(Medication medication)
        {
            medication.UpdatedAt = DateTime.UtcNow;
            
            _context.Medications.Update(medication);
            await _context.SaveChangesAsync();
            
            return medication;
        }

        public async Task<bool> DeleteMedicationAsync(Guid id, Guid userId)
        {
            var medication = await _context.Medications
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);
                
            if (medication == null)
            {
                return false;
            }
            
            _context.Medications.Remove(medication);
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<IEnumerable<MedicationSchedule>> GetMedicationSchedulesAsync(Guid medicationId)
        {
            return await _context.MedicationSchedules
                .Where(ms => ms.MedicationId == medicationId)
                .OrderBy(ms => ms.ScheduledTime)
                .ToListAsync();
        }

        public async Task<MedicationSchedule> GetMedicationScheduleByIdAsync(Guid id)
        {
            return await _context.MedicationSchedules
                .Include(ms => ms.MedicationDoses)
                .FirstOrDefaultAsync(ms => ms.Id == id);
        }

        public async Task<MedicationSchedule> CreateMedicationScheduleAsync(MedicationSchedule schedule)
        {
            schedule.Id = Guid.NewGuid();
            schedule.CreatedAt = DateTime.UtcNow;
            schedule.UpdatedAt = DateTime.UtcNow;
            
            _context.MedicationSchedules.Add(schedule);
            await _context.SaveChangesAsync();
            
            return schedule;
        }

        public async Task<MedicationSchedule> UpdateMedicationScheduleAsync(MedicationSchedule schedule)
        {
            schedule.UpdatedAt = DateTime.UtcNow;
            
            _context.MedicationSchedules.Update(schedule);
            await _context.SaveChangesAsync();
            
            return schedule;
        }

        public async Task<bool> DeleteMedicationScheduleAsync(Guid id)
        {
            var schedule = await _context.MedicationSchedules
                .FirstOrDefaultAsync(ms => ms.Id == id);
                
            if (schedule == null)
            {
                return false;
            }
            
            _context.MedicationSchedules.Remove(schedule);
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<IEnumerable<MedicationDose>> GetMedicationDosesAsync(Guid scheduleId)
        {
            return await _context.MedicationDoses
                .Where(md => md.ScheduleId == scheduleId)
                .OrderByDescending(md => md.ScheduledTime)
                .ToListAsync();
        }

        public async Task<MedicationDose> CreateMedicationDoseAsync(MedicationDose dose)
        {
            dose.Id = Guid.NewGuid();
            dose.CreatedAt = DateTime.UtcNow;
            dose.UpdatedAt = DateTime.UtcNow;
            
            _context.MedicationDoses.Add(dose);
            await _context.SaveChangesAsync();
            
            return dose;
        }

        public async Task<MedicationDose> UpdateMedicationDoseAsync(MedicationDose dose)
        {
            dose.UpdatedAt = DateTime.UtcNow;
            
            _context.MedicationDoses.Update(dose);
            await _context.SaveChangesAsync();
            
            return dose;
        }

        public async Task<bool> DeleteMedicationDoseAsync(Guid id)
        {
            var dose = await _context.MedicationDoses
                .FirstOrDefaultAsync(md => md.Id == id);
                
            if (dose == null)
            {
                return false;
            }
            
            _context.MedicationDoses.Remove(dose);
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<IEnumerable<Medication>> GetMedicationsDueForRefillAsync(Guid userId, int daysThreshold)
        {
            var thresholdDate = DateTime.UtcNow.AddDays(daysThreshold);
            
            return await _context.Medications
                .Include(m => m.Prescriptions)
                .Where(m => m.UserId == userId && 
                       m.IsActive && 
                       m.Prescriptions.Any(p => p.RefillDueDate <= thresholdDate))
                .OrderBy(m => m.Prescriptions.Min(p => p.RefillDueDate))
                .ToListAsync();
        }
    }
}

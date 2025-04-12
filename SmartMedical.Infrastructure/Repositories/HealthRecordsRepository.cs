using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartMedical.Core.Entities.HealthRecords;
using SmartMedical.Core.Interfaces;
using SmartMedical.Infrastructure.Data;

namespace SmartMedical.Infrastructure.Repositories
{
    public class HealthRecordsRepository : IHealthRecordsRepository
    {
        private readonly ApplicationDbContext _context;

        public HealthRecordsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Medical Conditions
        public async Task<IEnumerable<MedicalCondition>> GetMedicalConditionsAsync(Guid userId, string status = null, string search = null, int limit = 50, int offset = 0)
        {
            var query = _context.MedicalConditions.Where(c => c.UserId == userId);

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(c => c.Status == status);
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.Name.Contains(search) || c.Notes.Contains(search));
            }

            return await query
                .OrderByDescending(c => c.UpdatedAt)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<MedicalCondition> GetMedicalConditionByIdAsync(Guid id, Guid userId)
        {
            return await _context.MedicalConditions
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
        }

        public async Task<MedicalCondition> CreateMedicalConditionAsync(MedicalCondition condition)
        {
            condition.Id = Guid.NewGuid();
            condition.CreatedAt = DateTime.UtcNow;
            condition.UpdatedAt = DateTime.UtcNow;

            _context.MedicalConditions.Add(condition);
            await _context.SaveChangesAsync();

            return condition;
        }

        public async Task<MedicalCondition> UpdateMedicalConditionAsync(MedicalCondition condition)
        {
            condition.UpdatedAt = DateTime.UtcNow;

            _context.MedicalConditions.Update(condition);
            await _context.SaveChangesAsync();

            return condition;
        }

        public async Task<bool> DeleteMedicalConditionAsync(Guid id, Guid userId)
        {
            var condition = await _context.MedicalConditions
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (condition == null)
            {
                return false;
            }

            _context.MedicalConditions.Remove(condition);
            await _context.SaveChangesAsync();

            return true;
        }
        #endregion

        #region Allergies
        public async Task<IEnumerable<Allergy>> GetAllergiesAsync(Guid userId, string type = null, string search = null, int limit = 50, int offset = 0)
        {
            var query = _context.Allergies.Where(a => a.UserId == userId);

            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(a => a.Type == type);
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(a => a.Name.Contains(search) || a.Reaction.Contains(search) || a.Notes.Contains(search));
            }

            return await query
                .OrderByDescending(a => a.UpdatedAt)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<Allergy> GetAllergyByIdAsync(Guid id, Guid userId)
        {
            return await _context.Allergies
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);
        }

        public async Task<Allergy> CreateAllergyAsync(Allergy allergy)
        {
            allergy.Id = Guid.NewGuid();
            allergy.CreatedAt = DateTime.UtcNow;
            allergy.UpdatedAt = DateTime.UtcNow;

            _context.Allergies.Add(allergy);
            await _context.SaveChangesAsync();

            return allergy;
        }

        public async Task<Allergy> UpdateAllergyAsync(Allergy allergy)
        {
            allergy.UpdatedAt = DateTime.UtcNow;

            _context.Allergies.Update(allergy);
            await _context.SaveChangesAsync();

            return allergy;
        }

        public async Task<bool> DeleteAllergyAsync(Guid id, Guid userId)
        {
            var allergy = await _context.Allergies
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (allergy == null)
            {
                return false;
            }

            _context.Allergies.Remove(allergy);
            await _context.SaveChangesAsync();

            return true;
        }
        #endregion

        #region Vital Signs
        public async Task<IEnumerable<VitalSign>> GetVitalSignsAsync(Guid userId, string type = null, DateTime? startDate = null, DateTime? endDate = null, int limit = 50, int offset = 0)
        {
            var query = _context.VitalSigns.Where(v => v.UserId == userId);

            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(v => v.Type == type);
            }

            if (startDate.HasValue)
            {
                query = query.Where(v => v.MeasurementTime >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(v => v.MeasurementTime <= endDate.Value);
            }

            return await query
                .OrderByDescending(v => v.MeasurementTime)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<VitalSign> GetVitalSignByIdAsync(Guid id, Guid userId)
        {
            return await _context.VitalSigns
                .FirstOrDefaultAsync(v => v.Id == id && v.UserId == userId);
        }

        public async Task<VitalSign> CreateVitalSignAsync(VitalSign vitalSign)
        {
            vitalSign.Id = Guid.NewGuid();
            vitalSign.CreatedAt = DateTime.UtcNow;
            vitalSign.UpdatedAt = DateTime.UtcNow;

            _context.VitalSigns.Add(vitalSign);
            await _context.SaveChangesAsync();

            return vitalSign;
        }

        public async Task<VitalSign> UpdateVitalSignAsync(VitalSign vitalSign)
        {
            vitalSign.UpdatedAt = DateTime.UtcNow;

            _context.VitalSigns.Update(vitalSign);
            await _context.SaveChangesAsync();

            return vitalSign;
        }

        public async Task<bool> DeleteVitalSignAsync(Guid id, Guid userId)
        {
            var vitalSign = await _context.VitalSigns
                .FirstOrDefaultAsync(v => v.Id == id && v.UserId == userId);

            if (vitalSign == null)
            {
                return false;
            }

            _context.VitalSigns.Remove(vitalSign);
            await _context.SaveChangesAsync();

            return true;
        }
        #endregion

        #region Immunizations
        public async Task<IEnumerable<Immunization>> GetImmunizationsAsync(Guid userId, string search = null, int limit = 50, int offset = 0)
        {
            var query = _context.Immunizations.Where(i => i.UserId == userId);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(i => i.Name.Contains(search) || i.Notes.Contains(search));
            }

            return await query
                .OrderByDescending(i => i.AdministrationDate)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<Immunization> GetImmunizationByIdAsync(Guid id, Guid userId)
        {
            return await _context.Immunizations
                .FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);
        }

        public async Task<Immunization> CreateImmunizationAsync(Immunization immunization)
        {
            immunization.Id = Guid.NewGuid();
            immunization.CreatedAt = DateTime.UtcNow;
            immunization.UpdatedAt = DateTime.UtcNow;

            _context.Immunizations.Add(immunization);
            await _context.SaveChangesAsync();

            return immunization;
        }

        public async Task<Immunization> UpdateImmunizationAsync(Immunization immunization)
        {
            immunization.UpdatedAt = DateTime.UtcNow;

            _context.Immunizations.Update(immunization);
            await _context.SaveChangesAsync();

            return immunization;
        }

        public async Task<bool> DeleteImmunizationAsync(Guid id, Guid userId)
        {
            var immunization = await _context.Immunizations
                .FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);

            if (immunization == null)
            {
                return false;
            }

            _context.Immunizations.Remove(immunization);
            await _context.SaveChangesAsync();

            return true;
        }
        #endregion

        #region Family History
        public async Task<IEnumerable<FamilyHistory>> GetFamilyHistoryAsync(Guid userId, string relationship = null, string search = null, int limit = 50, int offset = 0)
        {
            var query = _context.FamilyHistory.Where(f => f.UserId == userId);

            if (!string.IsNullOrEmpty(relationship))
            {
                query = query.Where(f => f.Relationship == relationship);
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(f => f.Condition.Contains(search) || f.Notes.Contains(search));
            }

            return await query
                .OrderByDescending(f => f.UpdatedAt)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<FamilyHistory> GetFamilyHistoryByIdAsync(Guid id, Guid userId)
        {
            return await _context.FamilyHistory
                .FirstOrDefaultAsync(f => f.Id == id && f.UserId == userId);
        }

        public async Task<FamilyHistory> CreateFamilyHistoryAsync(FamilyHistory familyHistory)
        {
            familyHistory.Id = Guid.NewGuid();
            familyHistory.CreatedAt = DateTime.UtcNow;
            familyHistory.UpdatedAt = DateTime.UtcNow;

            _context.FamilyHistory.Add(familyHistory);
            await _context.SaveChangesAsync();

            return familyHistory;
        }

        public async Task<FamilyHistory> UpdateFamilyHistoryAsync(FamilyHistory familyHistory)
        {
            familyHistory.UpdatedAt = DateTime.UtcNow;

            _context.FamilyHistory.Update(familyHistory);
            await _context.SaveChangesAsync();

            return familyHistory;
        }

        public async Task<bool> DeleteFamilyHistoryAsync(Guid id, Guid userId)
        {
            var familyHistory = await _context.FamilyHistory
                .FirstOrDefaultAsync(f => f.Id == id && f.UserId == userId);

            if (familyHistory == null)
            {
                return false;
            }

            _context.FamilyHistory.Remove(familyHistory);
            await _context.SaveChangesAsync();

            return true;
        }
        #endregion
    }
}

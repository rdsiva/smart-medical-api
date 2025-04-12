using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartMedical.Business.Interfaces;
using SmartMedical.Core.Entities.HealthRecords;
using SmartMedical.Core.Interfaces;

namespace SmartMedical.Business.Services
{
    public class HealthRecordsService : IHealthRecordsService
    {
        private readonly IHealthRecordsRepository _healthRecordsRepository;

        public HealthRecordsService(IHealthRecordsRepository healthRecordsRepository)
        {
            _healthRecordsRepository = healthRecordsRepository;
        }

        #region Medical Conditions
        public async Task<IEnumerable<MedicalCondition>> GetMedicalConditionsAsync(Guid userId, string status = null, string search = null, int limit = 50, int offset = 0)
        {
            return await _healthRecordsRepository.GetMedicalConditionsAsync(userId, status, search, limit, offset);
        }

        public async Task<MedicalCondition> GetMedicalConditionByIdAsync(Guid id, Guid userId)
        {
            return await _healthRecordsRepository.GetMedicalConditionByIdAsync(id, userId);
        }

        public async Task<MedicalCondition> CreateMedicalConditionAsync(MedicalCondition condition)
        {
            return await _healthRecordsRepository.CreateMedicalConditionAsync(condition);
        }

        public async Task<MedicalCondition> UpdateMedicalConditionAsync(MedicalCondition condition)
        {
            var existingCondition = await _healthRecordsRepository.GetMedicalConditionByIdAsync(condition.Id, condition.UserId);
            if (existingCondition == null)
            {
                throw new KeyNotFoundException($"Medical condition with ID {condition.Id} not found");
            }

            return await _healthRecordsRepository.UpdateMedicalConditionAsync(condition);
        }

        public async Task<bool> DeleteMedicalConditionAsync(Guid id, Guid userId)
        {
            return await _healthRecordsRepository.DeleteMedicalConditionAsync(id, userId);
        }
        #endregion

        #region Allergies
        public async Task<IEnumerable<Allergy>> GetAllergiesAsync(Guid userId, string type = null, string search = null, int limit = 50, int offset = 0)
        {
            return await _healthRecordsRepository.GetAllergiesAsync(userId, type, search, limit, offset);
        }

        public async Task<Allergy> GetAllergyByIdAsync(Guid id, Guid userId)
        {
            return await _healthRecordsRepository.GetAllergyByIdAsync(id, userId);
        }

        public async Task<Allergy> CreateAllergyAsync(Allergy allergy)
        {
            return await _healthRecordsRepository.CreateAllergyAsync(allergy);
        }

        public async Task<Allergy> UpdateAllergyAsync(Allergy allergy)
        {
            var existingAllergy = await _healthRecordsRepository.GetAllergyByIdAsync(allergy.Id, allergy.UserId);
            if (existingAllergy == null)
            {
                throw new KeyNotFoundException($"Allergy with ID {allergy.Id} not found");
            }

            return await _healthRecordsRepository.UpdateAllergyAsync(allergy);
        }

        public async Task<bool> DeleteAllergyAsync(Guid id, Guid userId)
        {
            return await _healthRecordsRepository.DeleteAllergyAsync(id, userId);
        }
        #endregion

        #region Vital Signs
        public async Task<IEnumerable<VitalSign>> GetVitalSignsAsync(Guid userId, string type = null, DateTime? startDate = null, DateTime? endDate = null, int limit = 50, int offset = 0)
        {
            return await _healthRecordsRepository.GetVitalSignsAsync(userId, type, startDate, endDate, limit, offset);
        }

        public async Task<VitalSign> GetVitalSignByIdAsync(Guid id, Guid userId)
        {
            return await _healthRecordsRepository.GetVitalSignByIdAsync(id, userId);
        }

        public async Task<VitalSign> CreateVitalSignAsync(VitalSign vitalSign)
        {
            return await _healthRecordsRepository.CreateVitalSignAsync(vitalSign);
        }

        public async Task<VitalSign> UpdateVitalSignAsync(VitalSign vitalSign)
        {
            var existingVitalSign = await _healthRecordsRepository.GetVitalSignByIdAsync(vitalSign.Id, vitalSign.UserId);
            if (existingVitalSign == null)
            {
                throw new KeyNotFoundException($"Vital sign with ID {vitalSign.Id} not found");
            }

            return await _healthRecordsRepository.UpdateVitalSignAsync(vitalSign);
        }

        public async Task<bool> DeleteVitalSignAsync(Guid id, Guid userId)
        {
            return await _healthRecordsRepository.DeleteVitalSignAsync(id, userId);
        }
        #endregion

        #region Immunizations
        public async Task<IEnumerable<Immunization>> GetImmunizationsAsync(Guid userId, string search = null, int limit = 50, int offset = 0)
        {
            return await _healthRecordsRepository.GetImmunizationsAsync(userId, search, limit, offset);
        }

        public async Task<Immunization> GetImmunizationByIdAsync(Guid id, Guid userId)
        {
            return await _healthRecordsRepository.GetImmunizationByIdAsync(id, userId);
        }

        public async Task<Immunization> CreateImmunizationAsync(Immunization immunization)
        {
            return await _healthRecordsRepository.CreateImmunizationAsync(immunization);
        }

        public async Task<Immunization> UpdateImmunizationAsync(Immunization immunization)
        {
            var existingImmunization = await _healthRecordsRepository.GetImmunizationByIdAsync(immunization.Id, immunization.UserId);
            if (existingImmunization == null)
            {
                throw new KeyNotFoundException($"Immunization with ID {immunization.Id} not found");
            }

            return await _healthRecordsRepository.UpdateImmunizationAsync(immunization);
        }

        public async Task<bool> DeleteImmunizationAsync(Guid id, Guid userId)
        {
            return await _healthRecordsRepository.DeleteImmunizationAsync(id, userId);
        }
        #endregion

        #region Family History
        public async Task<IEnumerable<FamilyHistory>> GetFamilyHistoryAsync(Guid userId, string relationship = null, string search = null, int limit = 50, int offset = 0)
        {
            return await _healthRecordsRepository.GetFamilyHistoryAsync(userId, relationship, search, limit, offset);
        }

        public async Task<FamilyHistory> GetFamilyHistoryByIdAsync(Guid id, Guid userId)
        {
            return await _healthRecordsRepository.GetFamilyHistoryByIdAsync(id, userId);
        }

        public async Task<FamilyHistory> CreateFamilyHistoryAsync(FamilyHistory familyHistory)
        {
            return await _healthRecordsRepository.CreateFamilyHistoryAsync(familyHistory);
        }

        public async Task<FamilyHistory> UpdateFamilyHistoryAsync(FamilyHistory familyHistory)
        {
            var existingFamilyHistory = await _healthRecordsRepository.GetFamilyHistoryByIdAsync(familyHistory.Id, familyHistory.UserId);
            if (existingFamilyHistory == null)
            {
                throw new KeyNotFoundException($"Family history with ID {familyHistory.Id} not found");
            }

            return await _healthRecordsRepository.UpdateFamilyHistoryAsync(familyHistory);
        }

        public async Task<bool> DeleteFamilyHistoryAsync(Guid id, Guid userId)
        {
            return await _healthRecordsRepository.DeleteFamilyHistoryAsync(id, userId);
        }
        #endregion
    }
}

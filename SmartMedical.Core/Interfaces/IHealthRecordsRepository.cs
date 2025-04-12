using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartMedical.Core.Entities.HealthRecords;

namespace SmartMedical.Core.Interfaces
{
    public interface IHealthRecordsRepository
    {
        // Medical Conditions
        Task<IEnumerable<MedicalCondition>> GetMedicalConditionsAsync(Guid userId, string status = null, string search = null, int limit = 50, int offset = 0);
        Task<MedicalCondition> GetMedicalConditionByIdAsync(Guid id, Guid userId);
        Task<MedicalCondition> CreateMedicalConditionAsync(MedicalCondition condition);
        Task<MedicalCondition> UpdateMedicalConditionAsync(MedicalCondition condition);
        Task<bool> DeleteMedicalConditionAsync(Guid id, Guid userId);
        
        // Allergies
        Task<IEnumerable<Allergy>> GetAllergiesAsync(Guid userId, string type = null, string search = null, int limit = 50, int offset = 0);
        Task<Allergy> GetAllergyByIdAsync(Guid id, Guid userId);
        Task<Allergy> CreateAllergyAsync(Allergy allergy);
        Task<Allergy> UpdateAllergyAsync(Allergy allergy);
        Task<bool> DeleteAllergyAsync(Guid id, Guid userId);
        
        // Vital Signs
        Task<IEnumerable<VitalSign>> GetVitalSignsAsync(Guid userId, string type = null, DateTime? startDate = null, DateTime? endDate = null, int limit = 50, int offset = 0);
        Task<VitalSign> GetVitalSignByIdAsync(Guid id, Guid userId);
        Task<VitalSign> CreateVitalSignAsync(VitalSign vitalSign);
        Task<VitalSign> UpdateVitalSignAsync(VitalSign vitalSign);
        Task<bool> DeleteVitalSignAsync(Guid id, Guid userId);
        
        // Immunizations
        Task<IEnumerable<Immunization>> GetImmunizationsAsync(Guid userId, string search = null, int limit = 50, int offset = 0);
        Task<Immunization> GetImmunizationByIdAsync(Guid id, Guid userId);
        Task<Immunization> CreateImmunizationAsync(Immunization immunization);
        Task<Immunization> UpdateImmunizationAsync(Immunization immunization);
        Task<bool> DeleteImmunizationAsync(Guid id, Guid userId);
        
        // Family History
        Task<IEnumerable<FamilyHistory>> GetFamilyHistoryAsync(Guid userId, string relationship = null, string search = null, int limit = 50, int offset = 0);
        Task<FamilyHistory> GetFamilyHistoryByIdAsync(Guid id, Guid userId);
        Task<FamilyHistory> CreateFamilyHistoryAsync(FamilyHistory familyHistory);
        Task<FamilyHistory> UpdateFamilyHistoryAsync(FamilyHistory familyHistory);
        Task<bool> DeleteFamilyHistoryAsync(Guid id, Guid userId);
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartMedical.Core.Entities.Insurance;

namespace SmartMedical.Business.Interfaces
{
    public interface IInsuranceService
    {
        // Insurance Plans
        Task<IEnumerable<InsurancePlan>> GetInsurancePlansAsync(Guid userId, string planType = null, int limit = 50, int offset = 0);
        Task<InsurancePlan> GetInsurancePlanByIdAsync(Guid id, Guid userId);
        Task<InsurancePlan> CreateInsurancePlanAsync(InsurancePlan plan);
        Task<InsurancePlan> UpdateInsurancePlanAsync(InsurancePlan plan);
        Task<bool> DeleteInsurancePlanAsync(Guid id, Guid userId);
        
        // Insurance Coverage
        Task<IEnumerable<InsuranceCoverage>> GetInsuranceCoverageAsync(Guid insurancePlanId);
        Task<InsuranceCoverage> GetInsuranceCoverageByIdAsync(Guid id);
        Task<InsuranceCoverage> CreateInsuranceCoverageAsync(InsuranceCoverage coverage);
        Task<InsuranceCoverage> UpdateInsuranceCoverageAsync(InsuranceCoverage coverage);
        Task<bool> DeleteInsuranceCoverageAsync(Guid id);
        
        // Insurance Claims
        Task<IEnumerable<InsuranceClaim>> GetInsuranceClaimsAsync(Guid userId, Guid? insurancePlanId = null, string status = null, DateTime? startDate = null, DateTime? endDate = null, int limit = 50, int offset = 0);
        Task<InsuranceClaim> GetInsuranceClaimByIdAsync(Guid id, Guid userId);
        Task<InsuranceClaim> CreateInsuranceClaimAsync(InsuranceClaim claim);
        Task<InsuranceClaim> UpdateInsuranceClaimAsync(InsuranceClaim claim);
        Task<bool> DeleteInsuranceClaimAsync(Guid id, Guid userId);
        
        // Verification
        Task<(bool isActive, bool isEligible, InsuranceCoverage coverage, bool inNetwork, decimal? copayAmount, decimal? coinsurancePercentage, decimal? deductibleRemaining, decimal? outOfPocketRemaining, string verificationReference)> 
            VerifyInsuranceCoverageAsync(Guid insurancePlanId, Guid userId, DateTime serviceDate, string serviceType, Guid? providerId = null);
    }
}

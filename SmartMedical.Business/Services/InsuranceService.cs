using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartMedical.Business.Interfaces;
using SmartMedical.Core.Entities.Insurance;
using SmartMedical.Core.Interfaces;

namespace SmartMedical.Business.Services
{
    public class InsuranceService : IInsuranceService
    {
        private readonly IInsuranceRepository _insuranceRepository;

        public InsuranceService(IInsuranceRepository insuranceRepository)
        {
            _insuranceRepository = insuranceRepository;
        }

        #region Insurance Plans
        public async Task<IEnumerable<InsurancePlan>> GetInsurancePlansAsync(Guid userId, string planType = null, int limit = 50, int offset = 0)
        {
            return await _insuranceRepository.GetInsurancePlansAsync(userId, planType, limit, offset);
        }

        public async Task<InsurancePlan> GetInsurancePlanByIdAsync(Guid id, Guid userId)
        {
            return await _insuranceRepository.GetInsurancePlanByIdAsync(id, userId);
        }

        public async Task<InsurancePlan> CreateInsurancePlanAsync(InsurancePlan plan)
        {
            return await _insuranceRepository.CreateInsurancePlanAsync(plan);
        }

        public async Task<InsurancePlan> UpdateInsurancePlanAsync(InsurancePlan plan)
        {
            var existingPlan = await _insuranceRepository.GetInsurancePlanByIdAsync(plan.Id, plan.UserId);
            if (existingPlan == null)
            {
                throw new KeyNotFoundException($"Insurance plan with ID {plan.Id} not found");
            }

            return await _insuranceRepository.UpdateInsurancePlanAsync(plan);
        }

        public async Task<bool> DeleteInsurancePlanAsync(Guid id, Guid userId)
        {
            return await _insuranceRepository.DeleteInsurancePlanAsync(id, userId);
        }
        #endregion

        #region Insurance Coverage
        public async Task<IEnumerable<InsuranceCoverage>> GetInsuranceCoverageAsync(Guid insurancePlanId)
        {
            return await _insuranceRepository.GetInsuranceCoverageAsync(insurancePlanId);
        }

        public async Task<InsuranceCoverage> GetInsuranceCoverageByIdAsync(Guid id)
        {
            return await _insuranceRepository.GetInsuranceCoverageByIdAsync(id);
        }

        public async Task<InsuranceCoverage> CreateInsuranceCoverageAsync(InsuranceCoverage coverage)
        {
            return await _insuranceRepository.CreateInsuranceCoverageAsync(coverage);
        }

        public async Task<InsuranceCoverage> UpdateInsuranceCoverageAsync(InsuranceCoverage coverage)
        {
            var existingCoverage = await _insuranceRepository.GetInsuranceCoverageByIdAsync(coverage.Id);
            if (existingCoverage == null)
            {
                throw new KeyNotFoundException($"Insurance coverage with ID {coverage.Id} not found");
            }

            return await _insuranceRepository.UpdateInsuranceCoverageAsync(coverage);
        }

        public async Task<bool> DeleteInsuranceCoverageAsync(Guid id)
        {
            return await _insuranceRepository.DeleteInsuranceCoverageAsync(id);
        }
        #endregion

        #region Insurance Claims
        public async Task<IEnumerable<InsuranceClaim>> GetInsuranceClaimsAsync(Guid userId, Guid? insurancePlanId = null, string status = null, DateTime? startDate = null, DateTime? endDate = null, int limit = 50, int offset = 0)
        {
            return await _insuranceRepository.GetInsuranceClaimsAsync(userId, insurancePlanId, status, startDate, endDate, limit, offset);
        }

        public async Task<InsuranceClaim> GetInsuranceClaimByIdAsync(Guid id, Guid userId)
        {
            return await _insuranceRepository.GetInsuranceClaimByIdAsync(id, userId);
        }

        public async Task<InsuranceClaim> CreateInsuranceClaimAsync(InsuranceClaim claim)
        {
            return await _insuranceRepository.CreateInsuranceClaimAsync(claim);
        }

        public async Task<InsuranceClaim> UpdateInsuranceClaimAsync(InsuranceClaim claim)
        {
            var existingClaim = await _insuranceRepository.GetInsuranceClaimByIdAsync(claim.Id, claim.UserId);
            if (existingClaim == null)
            {
                throw new KeyNotFoundException($"Insurance claim with ID {claim.Id} not found");
            }

            return await _insuranceRepository.UpdateInsuranceClaimAsync(claim);
        }

        public async Task<bool> DeleteInsuranceClaimAsync(Guid id, Guid userId)
        {
            return await _insuranceRepository.DeleteInsuranceClaimAsync(id, userId);
        }
        #endregion

        #region Verification
        public async Task<(bool isActive, bool isEligible, InsuranceCoverage coverage, bool inNetwork, decimal? copayAmount, decimal? coinsurancePercentage, decimal? deductibleRemaining, decimal? outOfPocketRemaining, string verificationReference)> 
            VerifyInsuranceCoverageAsync(Guid insurancePlanId, Guid userId, DateTime serviceDate, string serviceType, Guid? providerId = null)
        {
            // In a real implementation, this would call an external insurance verification service
            // For now, we'll implement a simple mock verification

            var (isActive, isEligible) = await _insuranceRepository.VerifyInsuranceCoverageAsync(insurancePlanId, serviceDate, serviceType, providerId);

            if (!isActive || !isEligible)
            {
                return (isActive, isEligible, null, false, null, null, null, null, null);
            }

            // Get the insurance plan
            var plan = await _insuranceRepository.GetInsurancePlanByIdAsync(insurancePlanId, userId);
            if (plan == null)
            {
                return (false, false, null, false, null, null, null, null, null);
            }

            // Find the coverage for the service type
            var coverageDetails = plan.CoverageDetails?.FirstOrDefault(c => c.ServiceType == serviceType);
            
            // For demo purposes, we'll generate some mock values
            bool inNetwork = true; // Assume provider is in-network
            decimal? copayAmount = coverageDetails?.CopayAmount ?? 20.00m; // Default $20 copay
            decimal? coinsurancePercentage = coverageDetails?.CoinsurancePercentage ?? 20.00m; // Default 20% coinsurance
            decimal? deductibleRemaining = 500.00m; // Mock value
            decimal? outOfPocketRemaining = 2000.00m; // Mock value
            string verificationReference = $"VER-{DateTime.UtcNow.ToString("yyyyMMddHHmmss")}-{Guid.NewGuid().ToString().Substring(0, 8)}";

            return (isActive, isEligible, coverageDetails, inNetwork, copayAmount, coinsurancePercentage, deductibleRemaining, outOfPocketRemaining, verificationReference);
        }
        #endregion
    }
}

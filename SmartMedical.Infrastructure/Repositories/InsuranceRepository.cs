using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartMedical.Core.Entities.Insurance;
using SmartMedical.Core.Interfaces;
using SmartMedical.Infrastructure.Data;

namespace SmartMedical.Infrastructure.Repositories
{
    public class InsuranceRepository : IInsuranceRepository
    {
        private readonly ApplicationDbContext _context;

        public InsuranceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Insurance Plans
        public async Task<IEnumerable<InsurancePlan>> GetInsurancePlansAsync(Guid userId, string planType = null, int limit = 50, int offset = 0)
        {
            var query = _context.InsurancePlans.Where(p => p.UserId == userId);

            if (!string.IsNullOrEmpty(planType))
            {
                query = query.Where(p => p.PlanType == planType);
            }

            return await query
                .OrderByDescending(p => p.IsPrimary)
                .ThenByDescending(p => p.UpdatedAt)
                .Skip(offset)
                .Take(limit)
                .Include(p => p.CoverageDetails)
                .ToListAsync();
        }

        public async Task<InsurancePlan> GetInsurancePlanByIdAsync(Guid id, Guid userId)
        {
            return await _context.InsurancePlans
                .Include(p => p.CoverageDetails)
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);
        }

        public async Task<InsurancePlan> CreateInsurancePlanAsync(InsurancePlan plan)
        {
            plan.Id = Guid.NewGuid();
            plan.CreatedAt = DateTime.UtcNow;
            plan.UpdatedAt = DateTime.UtcNow;

            // If this is marked as primary, update other plans to not be primary
            if (plan.IsPrimary)
            {
                var existingPrimaryPlans = await _context.InsurancePlans
                    .Where(p => p.UserId == plan.UserId && p.IsPrimary)
                    .ToListAsync();

                foreach (var existingPlan in existingPrimaryPlans)
                {
                    existingPlan.IsPrimary = false;
                    existingPlan.UpdatedAt = DateTime.UtcNow;
                }
            }

            _context.InsurancePlans.Add(plan);
            await _context.SaveChangesAsync();

            return plan;
        }

        public async Task<InsurancePlan> UpdateInsurancePlanAsync(InsurancePlan plan)
        {
            plan.UpdatedAt = DateTime.UtcNow;

            // If this is marked as primary, update other plans to not be primary
            if (plan.IsPrimary)
            {
                var existingPrimaryPlans = await _context.InsurancePlans
                    .Where(p => p.UserId == plan.UserId && p.IsPrimary && p.Id != plan.Id)
                    .ToListAsync();

                foreach (var existingPlan in existingPrimaryPlans)
                {
                    existingPlan.IsPrimary = false;
                    existingPlan.UpdatedAt = DateTime.UtcNow;
                }
            }

            _context.InsurancePlans.Update(plan);
            await _context.SaveChangesAsync();

            return plan;
        }

        public async Task<bool> DeleteInsurancePlanAsync(Guid id, Guid userId)
        {
            var plan = await _context.InsurancePlans
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (plan == null)
            {
                return false;
            }

            _context.InsurancePlans.Remove(plan);
            await _context.SaveChangesAsync();

            return true;
        }
        #endregion

        #region Insurance Coverage
        public async Task<IEnumerable<InsuranceCoverage>> GetInsuranceCoverageAsync(Guid insurancePlanId)
        {
            return await _context.InsuranceCoverages
                .Where(c => c.InsurancePlanId == insurancePlanId)
                .OrderBy(c => c.ServiceType)
                .ToListAsync();
        }

        public async Task<InsuranceCoverage> GetInsuranceCoverageByIdAsync(Guid id)
        {
            return await _context.InsuranceCoverages
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<InsuranceCoverage> CreateInsuranceCoverageAsync(InsuranceCoverage coverage)
        {
            coverage.Id = Guid.NewGuid();
            coverage.CreatedAt = DateTime.UtcNow;
            coverage.UpdatedAt = DateTime.UtcNow;

            _context.InsuranceCoverages.Add(coverage);
            await _context.SaveChangesAsync();

            return coverage;
        }

        public async Task<InsuranceCoverage> UpdateInsuranceCoverageAsync(InsuranceCoverage coverage)
        {
            coverage.UpdatedAt = DateTime.UtcNow;

            _context.InsuranceCoverages.Update(coverage);
            await _context.SaveChangesAsync();

            return coverage;
        }

        public async Task<bool> DeleteInsuranceCoverageAsync(Guid id)
        {
            var coverage = await _context.InsuranceCoverages
                .FirstOrDefaultAsync(c => c.Id == id);

            if (coverage == null)
            {
                return false;
            }

            _context.InsuranceCoverages.Remove(coverage);
            await _context.SaveChangesAsync();

            return true;
        }
        #endregion

        #region Insurance Claims
        public async Task<IEnumerable<InsuranceClaim>> GetInsuranceClaimsAsync(Guid userId, Guid? insurancePlanId = null, string status = null, DateTime? startDate = null, DateTime? endDate = null, int limit = 50, int offset = 0)
        {
            var query = _context.InsuranceClaims.Where(c => c.UserId == userId);

            if (insurancePlanId.HasValue)
            {
                query = query.Where(c => c.InsurancePlanId == insurancePlanId.Value);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(c => c.Status == status);
            }

            if (startDate.HasValue)
            {
                query = query.Where(c => c.ServiceDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(c => c.ServiceDate <= endDate.Value);
            }

            return await query
                .OrderByDescending(c => c.ServiceDate)
                .Skip(offset)
                .Take(limit)
                .Include(c => c.InsurancePlan)
                .ToListAsync();
        }

        public async Task<InsuranceClaim> GetInsuranceClaimByIdAsync(Guid id, Guid userId)
        {
            return await _context.InsuranceClaims
                .Include(c => c.InsurancePlan)
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
        }

        public async Task<InsuranceClaim> CreateInsuranceClaimAsync(InsuranceClaim claim)
        {
            claim.Id = Guid.NewGuid();
            claim.CreatedAt = DateTime.UtcNow;
            claim.UpdatedAt = DateTime.UtcNow;

            _context.InsuranceClaims.Add(claim);
            await _context.SaveChangesAsync();

            return claim;
        }

        public async Task<InsuranceClaim> UpdateInsuranceClaimAsync(InsuranceClaim claim)
        {
            claim.UpdatedAt = DateTime.UtcNow;

            _context.InsuranceClaims.Update(claim);
            await _context.SaveChangesAsync();

            return claim;
        }

        public async Task<bool> DeleteInsuranceClaimAsync(Guid id, Guid userId)
        {
            var claim = await _context.InsuranceClaims
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (claim == null)
            {
                return false;
            }

            _context.InsuranceClaims.Remove(claim);
            await _context.SaveChangesAsync();

            return true;
        }
        #endregion

        #region Verification
        public async Task<(bool isActive, bool isEligible)> VerifyInsuranceCoverageAsync(Guid insurancePlanId, DateTime serviceDate, string serviceType, Guid? providerId = null)
        {
            // In a real implementation, this would call an external insurance verification service
            // For now, we'll implement a simple mock verification

            var plan = await _context.InsurancePlans
                .FirstOrDefaultAsync(p => p.Id == insurancePlanId);

            if (plan == null)
            {
                return (false, false);
            }

            bool isActive = plan.EffectiveDate <= serviceDate && 
                           (!plan.ExpirationDate.HasValue || plan.ExpirationDate.Value >= serviceDate);

            // For demo purposes, we'll assume the plan is eligible for all service types
            bool isEligible = isActive;

            return (isActive, isEligible);
        }
        #endregion
    }
}

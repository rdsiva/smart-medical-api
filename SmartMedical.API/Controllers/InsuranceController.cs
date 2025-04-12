using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartMedical.Business.Interfaces;
using SmartMedical.Core.Entities.Insurance;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartMedical.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InsuranceController : ControllerBase
    {
        private readonly ILogger<InsuranceController> _logger;
        private readonly IInsuranceService _insuranceService;

        public InsuranceController(
            ILogger<InsuranceController> logger,
            IInsuranceService insuranceService)
        {
            _logger = logger;
            _insuranceService = insuranceService;
        }

        #region Insurance Plans
        [HttpGet("plans")]
        public async Task<IActionResult> GetInsurancePlans(
            [FromQuery] string planType = null,
            [FromQuery] int limit = 50,
            [FromQuery] int offset = 0)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                // In a real application, this would come from the authenticated user
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var plans = await _insuranceService.GetInsurancePlansAsync(userId, planType, limit, offset);
                
                return Ok(new
                {
                    data = plans,
                    pagination = new
                    {
                        limit = limit,
                        offset = offset,
                        total = plans.Count
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving insurance plans");
                return BadRequest(new { message = "Failed to retrieve insurance plans" });
            }
        }

        [HttpGet("plans/{id}")]
        public async Task<IActionResult> GetInsurancePlan(Guid id)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var plan = await _insuranceService.GetInsurancePlanByIdAsync(id, userId);
                
                if (plan == null)
                {
                    return NotFound(new { message = "Insurance plan not found" });
                }
                
                return Ok(plan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving insurance plan");
                return BadRequest(new { message = "Failed to retrieve insurance plan" });
            }
        }

        [HttpPost("plans")]
        public async Task<IActionResult> CreateInsurancePlan([FromBody] InsurancePlanRequest request)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var plan = new InsurancePlan
                {
                    UserId = userId,
                    Provider = request.Provider,
                    PlanName = request.PlanName,
                    PlanType = request.PlanType,
                    MemberId = request.MemberId,
                    GroupNumber = request.GroupNumber,
                    SubscriberName = request.SubscriberName,
                    SubscriberRelationship = request.SubscriberRelationship,
                    EffectiveDate = request.EffectiveDate,
                    ExpirationDate = request.ExpirationDate,
                    IsPrimary = request.IsPrimary,
                    CardImageFront = request.CardImageFront,
                    CardImageBack = request.CardImageBack,
                    CustomerServicePhone = request.CustomerServicePhone,
                    ProviderPhone = request.ProviderPhone,
                    ClaimsAddressLine1 = request.ClaimsAddressLine1,
                    ClaimsAddressLine2 = request.ClaimsAddressLine2,
                    ClaimsAddressCity = request.ClaimsAddressCity,
                    ClaimsAddressState = request.ClaimsAddressState,
                    ClaimsAddressZip = request.ClaimsAddressZip,
                    ClaimsAddressCountry = request.ClaimsAddressCountry
                };

                var createdPlan = await _insuranceService.CreateInsurancePlanAsync(plan);
                
                return CreatedAtAction(nameof(GetInsurancePlan), new { id = createdPlan.Id }, createdPlan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating insurance plan");
                return BadRequest(new { message = "Failed to create insurance plan" });
            }
        }

        [HttpPut("plans/{id}")]
        public async Task<IActionResult> UpdateInsurancePlan(Guid id, [FromBody] InsurancePlanRequest request)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var existingPlan = await _insuranceService.GetInsurancePlanByIdAsync(id, userId);
                
                if (existingPlan == null)
                {
                    return NotFound(new { message = "Insurance plan not found" });
                }

                existingPlan.Provider = request.Provider ?? existingPlan.Provider;
                existingPlan.PlanName = request.PlanName ?? existingPlan.PlanName;
                existingPlan.PlanType = request.PlanType ?? existingPlan.PlanType;
                existingPlan.MemberId = request.MemberId ?? existingPlan.MemberId;
                existingPlan.GroupNumber = request.GroupNumber ?? existingPlan.GroupNumber;
                existingPlan.SubscriberName = request.SubscriberName ?? existingPlan.SubscriberName;
                existingPlan.SubscriberRelationship = request.SubscriberRelationship ?? existingPlan.SubscriberRelationship;
                existingPlan.EffectiveDate = request.EffectiveDate ?? existingPlan.EffectiveDate;
                existingPlan.ExpirationDate = request.ExpirationDate ?? existingPlan.ExpirationDate;
                existingPlan.IsPrimary = request.IsPrimary;
                existingPlan.CardImageFront = request.CardImageFront ?? existingPlan.CardImageFront;
                existingPlan.CardImageBack = request.CardImageBack ?? existingPlan.CardImageBack;
                existingPlan.CustomerServicePhone = request.CustomerServicePhone ?? existingPlan.CustomerServicePhone;
                existingPlan.ProviderPhone = request.ProviderPhone ?? existingPlan.ProviderPhone;
                existingPlan.ClaimsAddressLine1 = request.ClaimsAddressLine1 ?? existingPlan.ClaimsAddressLine1;
                existingPlan.ClaimsAddressLine2 = request.ClaimsAddressLine2 ?? existingPlan.ClaimsAddressLine2;
                existingPlan.ClaimsAddressCity = request.ClaimsAddressCity ?? existingPlan.ClaimsAddressCity;
                existingPlan.ClaimsAddressState = request.ClaimsAddressState ?? existingPlan.ClaimsAddressState;
                existingPlan.ClaimsAddressZip = request.ClaimsAddressZip ?? existingPlan.ClaimsAddressZip;
                existingPlan.ClaimsAddressCountry = request.ClaimsAddressCountry ?? existingPlan.ClaimsAddressCountry;

                var updatedPlan = await _insuranceService.UpdateInsurancePlanAsync(existingPlan);
                
                return Ok(updatedPlan);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Insurance plan not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating insurance plan");
                return BadRequest(new { message = "Failed to update insurance plan" });
            }
        }

        [HttpDelete("plans/{id}")]
        public async Task<IActionResult> DeleteInsurancePlan(Guid id)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var result = await _insuranceService.DeleteInsurancePlanAsync(id, userId);
                
                if (!result)
                {
                    return NotFound(new { message = "Insurance plan not found" });
                }
                
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting insurance plan");
                return BadRequest(new { message = "Failed to delete insurance plan" });
            }
        }
        #endregion

        #region Insurance Coverage
        [HttpGet("plans/{planId}/coverage")]
        public async Task<IActionResult> GetInsuranceCoverage(Guid planId)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                // Verify the plan belongs to the user
                var plan = await _insuranceService.GetInsurancePlanByIdAsync(planId, userId);
                if (plan == null)
                {
                    return NotFound(new { message = "Insurance plan not found" });
                }

                var coverage = await _insuranceService.GetInsuranceCoverageAsync(planId);
                
                return Ok(coverage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving insurance coverage");
                return BadRequest(new { message = "Failed to retrieve insurance coverage" });
            }
        }

        [HttpGet("coverage/{id}")]
        public async Task<IActionResult> GetInsuranceCoverageById(Guid id)
        {
            try
            {
                var coverage = await _insuranceService.GetInsuranceCoverageByIdAsync(id);
                
                if (coverage == null)
                {
                    return NotFound(new { message = "Insurance coverage not found" });
                }
                
                return Ok(coverage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving insurance coverage");
                return BadRequest(new { message = "Failed to retrieve insurance coverage" });
            }
        }

        [HttpPost("plans/{planId}/coverage")]
        public async Task<IActionResult> CreateInsuranceCoverage(Guid planId, [FromBody] InsuranceCoverageRequest request)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                // Verify the plan belongs to the user
                var plan = await _insuranceService.GetInsurancePlanByIdAsync(planId, userId);
                if (plan == null)
                {
                    return NotFound(new { message = "Insurance plan not found" });
                }

                var coverage = new InsuranceCoverage
                {
                    InsurancePlanId = planId,
                    ServiceType = request.ServiceType,
                    CopayAmount = request.CopayAmount,
                    CoinsurancePercentage = request.CoinsurancePercentage,
                    DeductibleAmount = request.DeductibleAmount,
                    OutOfPocketMaximum = request.OutOfPocketMaximum,
                    RequiresPreauthorization = request.RequiresPreauthorization,
                    RequiresReferral = request.RequiresReferral,
                    Notes = request.Notes
                };

                var createdCoverage = await _insuranceService.CreateInsuranceCoverageAsync(coverage);
                
                return CreatedAtAction(nameof(GetInsuranceCoverageById), new { id = createdCoverage.Id }, createdCoverage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating insurance coverage");
                return BadRequest(new { message = "Failed to create insurance coverage" });
            }
        }

        [HttpPut("coverage/{id}")]
        public async Task<IActionResult> UpdateInsuranceCoverage(Guid id, [FromBody] InsuranceCoverageRequest request)
        {
            try
            {
                var existingCoverage = await _insuranceService.GetInsuranceCoverageByIdAsync(id);
                
                if (existingCoverage == null)
                {
                    return NotFound(new { message = "Insurance coverage not found" });
                }

                existingCoverage.ServiceType = request.ServiceType ?? existingCoverage.ServiceType;
                existingCoverage.CopayAmount = request.CopayAmount ?? existingCoverage.CopayAmount;
                existingCoverage.CoinsurancePercentage = request.CoinsurancePercentage ?? existingCoverage.CoinsurancePercentage;
                existingCoverage.DeductibleAmount = request.DeductibleAmount ?? existingCoverage.DeductibleAmount;
                existingCoverage.OutOfPocketMaximum = request.OutOfPocketMaximum ?? existingCoverage.OutOfPocketMaximum;
                existingCoverage.RequiresPreauthorization = request.RequiresPreauthorization;
                existingCoverage.RequiresReferral = request.RequiresReferral;
                existingCoverage.Notes = request.Notes ?? existingCoverage.Notes;

                var updatedCoverage = await _insuranceService.UpdateInsuranceCoverageAsync(existingCoverage);
                
                return Ok(updatedCoverage);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Insurance coverage not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating insurance coverage");
                return BadRequest(new { message = "Failed to update insurance coverage" });
            }
        }

        [HttpDelete("coverage/{id}")]
        public async Task<IActionResult> DeleteInsuranceCoverage(Guid id)
        {
            try
            {
                var result = await _insuranceService.DeleteInsuranceCoverageAsync(id);
                
                if (!result)
                {
                    return NotFound(new { message = "Insurance coverage not found" });
                }
                
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting insurance coverage");
                return BadRequest(new { message = "Failed to delete insurance coverage" });
            }
        }
        #endregion

        #region Insurance Claims
        [HttpGet("claims")]
        public async Task<IActionResult> GetInsuranceClaims(
            [FromQuery] Guid? planId = null,
            [FromQuery] string status = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] int limit = 50,
            [FromQuery] int offset = 0)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var claims = await _insuranceService.GetInsuranceClaimsAsync(userId, planId, status, startDate, endDate, limit, offset);
                
                return Ok(new
                {
                    data = claims,
                    pagination = new
                    {
                        limit = limit,
                        offset = offset,
                        total = claims.Count
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving insurance claims");
                return BadRequest(new { message = "Failed to retrieve insurance claims" });
            }
        }

        [HttpGet("claims/{id}")]
        public async Task<IActionResult> GetInsuranceClaim(Guid id)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var claim = await _insuranceService.GetInsuranceClaimByIdAsync(id, userId);
                
                if (claim == null)
                {
                    return NotFound(new { message = "Insurance claim not found" });
                }
                
                return Ok(claim);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving insurance claim");
                return BadRequest(new { message = "Failed to retrieve insurance claim" });
            }
        }

        [HttpPost("claims")]
        public async Task<IActionResult> CreateInsuranceClaim([FromBody] InsuranceClaimRequest request)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                // Verify the plan belongs to the user
                var plan = await _insuranceService.GetInsurancePlanByIdAsync(request.InsurancePlanId, userId);
                if (plan == null)
                {
                    return NotFound(new { message = "Insurance plan not found" });
                }

                var claim = new InsuranceClaim
                {
                    UserId = userId,
                    InsurancePlanId = request.InsurancePlanId,
                    ClaimNumber = request.ClaimNumber,
                    ServiceDate = request.ServiceDate,
                    ServiceType = request.ServiceType,
                    ProviderName = request.ProviderName,
                    ProviderNpi = request.ProviderNpi,
                    DiagnosisCodes = request.DiagnosisCodes,
                    ProcedureCodes = request.ProcedureCodes,
                    BilledAmount = request.BilledAmount,
                    AllowedAmount = request.AllowedAmount,
                    PaidAmount = request.PaidAmount,
                    PatientResponsibility = request.PatientResponsibility,
                    Status = request.Status,
                    SubmissionDate = request.SubmissionDate,
                    ProcessedDate = request.ProcessedDate,
                    DenialReason = request.DenialReason,
                    Notes = request.Notes,
                    DocumentUrl = request.DocumentUrl
                };

                var createdClaim = await _insuranceService.CreateInsuranceClaimAsync(claim);
                
                return CreatedAtAction(nameof(GetInsuranceClaim), new { id = createdClaim.Id }, createdClaim);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating insurance claim");
                return BadRequest(new { message = "Failed to create insurance claim" });
            }
        }

        [HttpPut("claims/{id}")]
        public async Task<IActionResult> UpdateInsuranceClaim(Guid id, [FromBody] InsuranceClaimRequest request)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var existingClaim = await _insuranceService.GetInsuranceClaimByIdAsync(id, userId);
                
                if (existingClaim == null)
                {
                    return NotFound(new { message = "Insurance claim not found" });
                }

                // If the insurance plan ID is changing, verify the new plan belongs to the user
                if (request.InsurancePlanId != existingClaim.InsurancePlanId)
                {
                    var plan = await _insuranceService.GetInsurancePlanByIdAsync(request.InsurancePlanId, userId);
                    if (plan == null)
                    {
                        return NotFound(new { message = "Insurance plan not found" });
                    }
                }

                existingClaim.InsurancePlanId = request.InsurancePlanId;
                existingClaim.ClaimNumber = request.ClaimNumber ?? existingClaim.ClaimNumber;
                existingClaim.ServiceDate = request.ServiceDate;
                existingClaim.ServiceType = request.ServiceType ?? existingClaim.ServiceType;
                existingClaim.ProviderName = request.ProviderName ?? existingClaim.ProviderName;
                existingClaim.ProviderNpi = request.ProviderNpi ?? existingClaim.ProviderNpi;
                existingClaim.DiagnosisCodes = request.DiagnosisCodes ?? existingClaim.DiagnosisCodes;
                existingClaim.ProcedureCodes = request.ProcedureCodes ?? existingClaim.ProcedureCodes;
                existingClaim.BilledAmount = request.BilledAmount;
                existingClaim.AllowedAmount = request.AllowedAmount ?? existingClaim.AllowedAmount;
                existingClaim.PaidAmount = request.PaidAmount ?? existingClaim.PaidAmount;
                existingClaim.PatientResponsibility = request.PatientResponsibility ?? existingClaim.PatientResponsibility;
                existingClaim.Status = request.Status ?? existingClaim.Status;
                existingClaim.SubmissionDate = request.SubmissionDate ?? existingClaim.SubmissionDate;
                existingClaim.ProcessedDate = request.ProcessedDate ?? existingClaim.ProcessedDate;
                existingClaim.DenialReason = request.DenialReason ?? existingClaim.DenialReason;
                existingClaim.Notes = request.Notes ?? existingClaim.Notes;
                existingClaim.DocumentUrl = request.DocumentUrl ?? existingClaim.DocumentUrl;

                var updatedClaim = await _insuranceService.UpdateInsuranceClaimAsync(existingClaim);
                
                return Ok(updatedClaim);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Insurance claim not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating insurance claim");
                return BadRequest(new { message = "Failed to update insurance claim" });
            }
        }

        [HttpDelete("claims/{id}")]
        public async Task<IActionResult> DeleteInsuranceClaim(Guid id)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var result = await _insuranceService.DeleteInsuranceClaimAsync(id, userId);
                
                if (!result)
                {
                    return NotFound(new { message = "Insurance claim not found" });
                }
                
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting insurance claim");
                return BadRequest(new { message = "Failed to delete insurance claim" });
            }
        }
        #endregion

        #region Verification
        [HttpPost("plans/{planId}/verify")]
        public async Task<IActionResult> VerifyInsuranceCoverage(Guid planId, [FromBody] VerificationRequest request)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                // Verify the plan belongs to the user
                var plan = await _insuranceService.GetInsurancePlanByIdAsync(planId, userId);
                if (plan == null)
                {
                    return NotFound(new { message = "Insurance plan not found" });
                }

                var serviceDate = request.ServiceDate ?? DateTime.UtcNow;
                
                var (isActive, isEligible, coverage, inNetwork, copayAmount, coinsurancePercentage, deductibleRemaining, outOfPocketRemaining, verificationReference) = 
                    await _insuranceService.VerifyInsuranceCoverageAsync(planId, userId, serviceDate, request.ServiceType, request.ProviderId);
                
                return Ok(new
                {
                    isActive = isActive,
                    isEligible = isEligible,
                    coverageDetails = coverage,
                    inNetwork = inNetwork,
                    copayAmount = copayAmount,
                    coinsurancePercentage = coinsurancePercentage,
                    deductibleRemaining = deductibleRemaining,
                    outOfPocketRemaining = outOfPocketRemaining,
                    verificationDate = DateTime.UtcNow,
                    verificationReference = verificationReference,
                    notes = "This is a mock verification for demonstration purposes."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying insurance coverage");
                return BadRequest(new { message = "Failed to verify insurance coverage" });
            }
        }
        #endregion
    }

    #region Request Models
    public class InsurancePlanRequest
    {
        public string Provider { get; set; }
        public string PlanName { get; set; }
        public string PlanType { get; set; } // hmo, ppo, epo, pos, hdhp, medicare, medicaid, other
        public string MemberId { get; set; }
        public string GroupNumber { get; set; }
        public string SubscriberName { get; set; }
        public string SubscriberRelationship { get; set; } // self, spouse, parent, other
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool IsPrimary { get; set; }
        public string CardImageFront { get; set; }
        public string CardImageBack { get; set; }
        public string CustomerServicePhone { get; set; }
        public string ProviderPhone { get; set; }
        public string ClaimsAddressLine1 { get; set; }
        public string ClaimsAddressLine2 { get; set; }
        public string ClaimsAddressCity { get; set; }
        public string ClaimsAddressState { get; set; }
        public string ClaimsAddressZip { get; set; }
        public string ClaimsAddressCountry { get; set; }
    }

    public class InsuranceCoverageRequest
    {
        public string ServiceType { get; set; } // office_visit, specialist_visit, urgent_care, emergency, lab_work, imaging, procedure, prescription, other
        public decimal? CopayAmount { get; set; }
        public decimal? CoinsurancePercentage { get; set; }
        public decimal? DeductibleAmount { get; set; }
        public decimal? OutOfPocketMaximum { get; set; }
        public bool RequiresPreauthorization { get; set; }
        public bool RequiresReferral { get; set; }
        public string Notes { get; set; }
    }

    public class InsuranceClaimRequest
    {
        public Guid InsurancePlanId { get; set; }
        public string ClaimNumber { get; set; }
        public DateTime ServiceDate { get; set; }
        public string ServiceType { get; set; } // office_visit, specialist_visit, urgent_care, emergency, lab_work, imaging, procedure, other
        public string ProviderName { get; set; }
        public string ProviderNpi { get; set; }
        public string DiagnosisCodes { get; set; } // Comma-separated ICD-10 codes
        public string ProcedureCodes { get; set; } // Comma-separated CPT/HCPCS codes
        public decimal BilledAmount { get; set; }
        public decimal? AllowedAmount { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? PatientResponsibility { get; set; }
        public string Status { get; set; } // submitted, in_process, denied, partially_paid, paid, appealed
        public DateTime? SubmissionDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public string DenialReason { get; set; }
        public string Notes { get; set; }
        public string DocumentUrl { get; set; }
    }

    public class VerificationRequest
    {
        public DateTime? ServiceDate { get; set; }
        public string ServiceType { get; set; } // office_visit, specialist_visit, urgent_care, emergency, lab_work, imaging, procedure, other
        public Guid? ProviderId { get; set; }
    }
    #endregion
}

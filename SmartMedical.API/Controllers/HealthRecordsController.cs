using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartMedical.Business.Interfaces;
using SmartMedical.Core.Entities.HealthRecords;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartMedical.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthRecordsController : ControllerBase
    {
        private readonly ILogger<HealthRecordsController> _logger;
        private readonly IHealthRecordsService _healthRecordsService;

        public HealthRecordsController(
            ILogger<HealthRecordsController> logger,
            IHealthRecordsService healthRecordsService)
        {
            _logger = logger;
            _healthRecordsService = healthRecordsService;
        }

        #region Medical Conditions
        [HttpGet("conditions")]
        public async Task<IActionResult> GetMedicalConditions(
            [FromQuery] string status = null,
            [FromQuery] string search = null,
            [FromQuery] int limit = 50,
            [FromQuery] int offset = 0)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                // In a real application, this would come from the authenticated user
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var conditions = await _healthRecordsService.GetMedicalConditionsAsync(userId, status, search, limit, offset);
                
                return Ok(new
                {
                    data = conditions,
                    pagination = new
                    {
                        limit = limit,
                        offset = offset,
                        total = conditions.Count
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving medical conditions");
                return BadRequest(new { message = "Failed to retrieve medical conditions" });
            }
        }

        [HttpGet("conditions/{id}")]
        public async Task<IActionResult> GetMedicalCondition(Guid id)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var condition = await _healthRecordsService.GetMedicalConditionByIdAsync(id, userId);
                
                if (condition == null)
                {
                    return NotFound(new { message = "Medical condition not found" });
                }
                
                return Ok(condition);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving medical condition");
                return BadRequest(new { message = "Failed to retrieve medical condition" });
            }
        }

        [HttpPost("conditions")]
        public async Task<IActionResult> CreateMedicalCondition([FromBody] MedicalConditionRequest request)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var condition = new MedicalCondition
                {
                    UserId = userId,
                    Name = request.Name,
                    Code = request.Code,
                    Status = request.Status,
                    DiagnosedDate = request.DiagnosedDate,
                    ResolvedDate = request.ResolvedDate,
                    Severity = request.Severity,
                    Notes = request.Notes,
                    DiagnosedById = request.DiagnosedById,
                    Source = request.Source
                };

                var createdCondition = await _healthRecordsService.CreateMedicalConditionAsync(condition);
                
                return CreatedAtAction(nameof(GetMedicalCondition), new { id = createdCondition.Id }, createdCondition);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating medical condition");
                return BadRequest(new { message = "Failed to create medical condition" });
            }
        }

        [HttpPut("conditions/{id}")]
        public async Task<IActionResult> UpdateMedicalCondition(Guid id, [FromBody] MedicalConditionRequest request)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var existingCondition = await _healthRecordsService.GetMedicalConditionByIdAsync(id, userId);
                
                if (existingCondition == null)
                {
                    return NotFound(new { message = "Medical condition not found" });
                }

                existingCondition.Name = request.Name ?? existingCondition.Name;
                existingCondition.Code = request.Code ?? existingCondition.Code;
                existingCondition.Status = request.Status ?? existingCondition.Status;
                existingCondition.DiagnosedDate = request.DiagnosedDate ?? existingCondition.DiagnosedDate;
                existingCondition.ResolvedDate = request.ResolvedDate ?? existingCondition.ResolvedDate;
                existingCondition.Severity = request.Severity ?? existingCondition.Severity;
                existingCondition.Notes = request.Notes ?? existingCondition.Notes;
                existingCondition.DiagnosedById = request.DiagnosedById ?? existingCondition.DiagnosedById;
                existingCondition.Source = request.Source ?? existingCondition.Source;

                var updatedCondition = await _healthRecordsService.UpdateMedicalConditionAsync(existingCondition);
                
                return Ok(updatedCondition);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Medical condition not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating medical condition");
                return BadRequest(new { message = "Failed to update medical condition" });
            }
        }

        [HttpDelete("conditions/{id}")]
        public async Task<IActionResult> DeleteMedicalCondition(Guid id)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var result = await _healthRecordsService.DeleteMedicalConditionAsync(id, userId);
                
                if (!result)
                {
                    return NotFound(new { message = "Medical condition not found" });
                }
                
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting medical condition");
                return BadRequest(new { message = "Failed to delete medical condition" });
            }
        }
        #endregion

        #region Allergies
        [HttpGet("allergies")]
        public async Task<IActionResult> GetAllergies(
            [FromQuery] string type = null,
            [FromQuery] string search = null,
            [FromQuery] int limit = 50,
            [FromQuery] int offset = 0)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var allergies = await _healthRecordsService.GetAllergiesAsync(userId, type, search, limit, offset);
                
                return Ok(new
                {
                    data = allergies,
                    pagination = new
                    {
                        limit = limit,
                        offset = offset,
                        total = allergies.Count
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving allergies");
                return BadRequest(new { message = "Failed to retrieve allergies" });
            }
        }

        [HttpGet("allergies/{id}")]
        public async Task<IActionResult> GetAllergy(Guid id)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var allergy = await _healthRecordsService.GetAllergyByIdAsync(id, userId);
                
                if (allergy == null)
                {
                    return NotFound(new { message = "Allergy not found" });
                }
                
                return Ok(allergy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving allergy");
                return BadRequest(new { message = "Failed to retrieve allergy" });
            }
        }

        [HttpPost("allergies")]
        public async Task<IActionResult> CreateAllergy([FromBody] AllergyRequest request)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var allergy = new Allergy
                {
                    UserId = userId,
                    Name = request.Name,
                    Type = request.Type,
                    Severity = request.Severity,
                    Reaction = request.Reaction,
                    DiagnosedDate = request.DiagnosedDate,
                    Notes = request.Notes,
                    Source = request.Source
                };

                var createdAllergy = await _healthRecordsService.CreateAllergyAsync(allergy);
                
                return CreatedAtAction(nameof(GetAllergy), new { id = createdAllergy.Id }, createdAllergy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating allergy");
                return BadRequest(new { message = "Failed to create allergy" });
            }
        }

        [HttpPut("allergies/{id}")]
        public async Task<IActionResult> UpdateAllergy(Guid id, [FromBody] AllergyRequest request)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var existingAllergy = await _healthRecordsService.GetAllergyByIdAsync(id, userId);
                
                if (existingAllergy == null)
                {
                    return NotFound(new { message = "Allergy not found" });
                }

                existingAllergy.Name = request.Name ?? existingAllergy.Name;
                existingAllergy.Type = request.Type ?? existingAllergy.Type;
                existingAllergy.Severity = request.Severity ?? existingAllergy.Severity;
                existingAllergy.Reaction = request.Reaction ?? existingAllergy.Reaction;
                existingAllergy.DiagnosedDate = request.DiagnosedDate ?? existingAllergy.DiagnosedDate;
                existingAllergy.Notes = request.Notes ?? existingAllergy.Notes;
                existingAllergy.Source = request.Source ?? existingAllergy.Source;

                var updatedAllergy = await _healthRecordsService.UpdateAllergyAsync(existingAllergy);
                
                return Ok(updatedAllergy);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Allergy not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating allergy");
                return BadRequest(new { message = "Failed to update allergy" });
            }
        }

        [HttpDelete("allergies/{id}")]
        public async Task<IActionResult> DeleteAllergy(Guid id)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var result = await _healthRecordsService.DeleteAllergyAsync(id, userId);
                
                if (!result)
                {
                    return NotFound(new { message = "Allergy not found" });
                }
                
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting allergy");
                return BadRequest(new { message = "Failed to delete allergy" });
            }
        }
        #endregion

        #region Vital Signs
        [HttpGet("vital-signs")]
        public async Task<IActionResult> GetVitalSigns(
            [FromQuery] string type = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] int limit = 50,
            [FromQuery] int offset = 0)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var vitalSigns = await _healthRecordsService.GetVitalSignsAsync(userId, type, startDate, endDate, limit, offset);
                
                return Ok(new
                {
                    data = vitalSigns,
                    pagination = new
                    {
                        limit = limit,
                        offset = offset,
                        total = vitalSigns.Count
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vital signs");
                return BadRequest(new { message = "Failed to retrieve vital signs" });
            }
        }

        [HttpGet("vital-signs/{id}")]
        public async Task<IActionResult> GetVitalSign(Guid id)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var vitalSign = await _healthRecordsService.GetVitalSignByIdAsync(id, userId);
                
                if (vitalSign == null)
                {
                    return NotFound(new { message = "Vital sign not found" });
                }
                
                return Ok(vitalSign);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vital sign");
                return BadRequest(new { message = "Failed to retrieve vital sign" });
            }
        }

        [HttpPost("vital-signs")]
        public async Task<IActionResult> CreateVitalSign([FromBody] VitalSignRequest request)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var vitalSign = new VitalSign
                {
                    UserId = userId,
                    Type = request.Type,
                    Value = request.Value,
                    Unit = request.Unit,
                    SecondaryValue = request.SecondaryValue,
                    SecondaryUnit = request.SecondaryUnit,
                    MeasurementTime = request.MeasurementTime ?? DateTime.UtcNow,
                    Source = request.Source,
                    DeviceId = request.DeviceId,
                    Notes = request.Notes
                };

                var createdVitalSign = await _healthRecordsService.CreateVitalSignAsync(vitalSign);
                
                return CreatedAtAction(nameof(GetVitalSign), new { id = createdVitalSign.Id }, createdVitalSign);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating vital sign");
                return BadRequest(new { message = "Failed to create vital sign" });
            }
        }

        [HttpPut("vital-signs/{id}")]
        public async Task<IActionResult> UpdateVitalSign(Guid id, [FromBody] VitalSignRequest request)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var existingVitalSign = await _healthRecordsService.GetVitalSignByIdAsync(id, userId);
                
                if (existingVitalSign == null)
                {
                    return NotFound(new { message = "Vital sign not found" });
                }

                existingVitalSign.Type = request.Type ?? existingVitalSign.Type;
                existingVitalSign.Value = request.Value;
                existingVitalSign.Unit = request.Unit ?? existingVitalSign.Unit;
                existingVitalSign.SecondaryValue = request.SecondaryValue ?? existingVitalSign.SecondaryValue;
                existingVitalSign.SecondaryUnit = request.SecondaryUnit ?? existingVitalSign.SecondaryUnit;
                existingVitalSign.MeasurementTime = request.MeasurementTime ?? existingVitalSign.MeasurementTime;
                existingVitalSign.Source = request.Source ?? existingVitalSign.Source;
                existingVitalSign.DeviceId = request.DeviceId ?? existingVitalSign.DeviceId;
                existingVitalSign.Notes = request.Notes ?? existingVitalSign.Notes;

                var updatedVitalSign = await _healthRecordsService.UpdateVitalSignAsync(existingVitalSign);
                
                return Ok(updatedVitalSign);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Vital sign not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating vital sign");
                return BadRequest(new { message = "Failed to update vital sign" });
            }
        }

        [HttpDelete("vital-signs/{id}")]
        public async Task<IActionResult> DeleteVitalSign(Guid id)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var result = await _healthRecordsService.DeleteVitalSignAsync(id, userId);
                
                if (!result)
                {
                    return NotFound(new { message = "Vital sign not found" });
                }
                
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting vital sign");
                return BadRequest(new { message = "Failed to delete vital sign" });
            }
        }
        #endregion

        #region Immunizations
        [HttpGet("immunizations")]
        public async Task<IActionResult> GetImmunizations(
            [FromQuery] string search = null,
            [FromQuery] int limit = 50,
            [FromQuery] int offset = 0)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var immunizations = await _healthRecordsService.GetImmunizationsAsync(userId, search, limit, offset);
                
                return Ok(new
                {
                    data = immunizations,
                    pagination = new
                    {
                        limit = limit,
                        offset = offset,
                        total = immunizations.Count
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving immunizations");
                return BadRequest(new { message = "Failed to retrieve immunizations" });
            }
        }

        [HttpGet("immunizations/{id}")]
        public async Task<IActionResult> GetImmunization(Guid id)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var immunization = await _healthRecordsService.GetImmunizationByIdAsync(id, userId);
                
                if (immunization == null)
                {
                    return NotFound(new { message = "Immunization not found" });
                }
                
                return Ok(immunization);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving immunization");
                return BadRequest(new { message = "Failed to retrieve immunization" });
            }
        }

        [HttpPost("immunizations")]
        public async Task<IActionResult> CreateImmunization([FromBody] ImmunizationRequest request)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var immunization = new Immunization
                {
                    UserId = userId,
                    Name = request.Name,
                    VaccineCode = request.VaccineCode,
                    AdministrationDate = request.AdministrationDate,
                    Manufacturer = request.Manufacturer,
                    LotNumber = request.LotNumber,
                    AdministeredBy = request.AdministeredBy,
                    AdministrationSite = request.AdministrationSite,
                    AdministrationRoute = request.AdministrationRoute,
                    DoseQuantity = request.DoseQuantity,
                    DoseUnit = request.DoseUnit,
                    DoseNumber = request.DoseNumber,
                    TotalDoses = request.TotalDoses,
                    ExpirationDate = request.ExpirationDate,
                    Notes = request.Notes,
                    Source = request.Source
                };

                var createdImmunization = await _healthRecordsService.CreateImmunizationAsync(immunization);
                
                return CreatedAtAction(nameof(GetImmunization), new { id = createdImmunization.Id }, createdImmunization);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating immunization");
                return BadRequest(new { message = "Failed to create immunization" });
            }
        }

        [HttpPut("immunizations/{id}")]
        public async Task<IActionResult> UpdateImmunization(Guid id, [FromBody] ImmunizationRequest request)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var existingImmunization = await _healthRecordsService.GetImmunizationByIdAsync(id, userId);
                
                if (existingImmunization == null)
                {
                    return NotFound(new { message = "Immunization not found" });
                }

                existingImmunization.Name = request.Name ?? existingImmunization.Name;
                existingImmunization.VaccineCode = request.VaccineCode ?? existingImmunization.VaccineCode;
                existingImmunization.AdministrationDate = request.AdministrationDate;
                existingImmunization.Manufacturer = request.Manufacturer ?? existingImmunization.Manufacturer;
                existingImmunization.LotNumber = request.LotNumber ?? existingImmunization.LotNumber;
                existingImmunization.AdministeredBy = request.AdministeredBy ?? existingImmunization.AdministeredBy;
                existingImmunization.AdministrationSite = request.AdministrationSite ?? existingImmunization.AdministrationSite;
                existingImmunization.AdministrationRoute = request.AdministrationRoute ?? existingImmunization.AdministrationRoute;
                existingImmunization.DoseQuantity = request.DoseQuantity ?? existingImmunization.DoseQuantity;
                existingImmunization.DoseUnit = request.DoseUnit ?? existingImmunization.DoseUnit;
                existingImmunization.DoseNumber = request.DoseNumber ?? existingImmunization.DoseNumber;
                existingImmunization.TotalDoses = request.TotalDoses ?? existingImmunization.TotalDoses;
                existingImmunization.ExpirationDate = request.ExpirationDate ?? existingImmunization.ExpirationDate;
                existingImmunization.Notes = request.Notes ?? existingImmunization.Notes;
                existingImmunization.Source = request.Source ?? existingImmunization.Source;

                var updatedImmunization = await _healthRecordsService.UpdateImmunizationAsync(existingImmunization);
                
                return Ok(updatedImmunization);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Immunization not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating immunization");
                return BadRequest(new { message = "Failed to update immunization" });
            }
        }

        [HttpDelete("immunizations/{id}")]
        public async Task<IActionResult> DeleteImmunization(Guid id)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var result = await _healthRecordsService.DeleteImmunizationAsync(id, userId);
                
                if (!result)
                {
                    return NotFound(new { message = "Immunization not found" });
                }
                
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting immunization");
                return BadRequest(new { message = "Failed to delete immunization" });
            }
        }
        #endregion

        #region Family History
        [HttpGet("family-history")]
        public async Task<IActionResult> GetFamilyHistory(
            [FromQuery] string relationship = null,
            [FromQuery] string search = null,
            [FromQuery] int limit = 50,
            [FromQuery] int offset = 0)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var familyHistory = await _healthRecordsService.GetFamilyHistoryAsync(userId, relationship, search, limit, offset);
                
                return Ok(new
                {
                    data = familyHistory,
                    pagination = new
                    {
                        limit = limit,
                        offset = offset,
                        total = familyHistory.Count
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving family history");
                return BadRequest(new { message = "Failed to retrieve family history" });
            }
        }

        [HttpGet("family-history/{id}")]
        public async Task<IActionResult> GetFamilyHistoryItem(Guid id)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var familyHistoryItem = await _healthRecordsService.GetFamilyHistoryByIdAsync(id, userId);
                
                if (familyHistoryItem == null)
                {
                    return NotFound(new { message = "Family history item not found" });
                }
                
                return Ok(familyHistoryItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving family history item");
                return BadRequest(new { message = "Failed to retrieve family history item" });
            }
        }

        [HttpPost("family-history")]
        public async Task<IActionResult> CreateFamilyHistory([FromBody] FamilyHistoryRequest request)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var familyHistory = new FamilyHistory
                {
                    UserId = userId,
                    Condition = request.Condition,
                    Relationship = request.Relationship,
                    Status = request.Status,
                    AgeAtDiagnosis = request.AgeAtDiagnosis,
                    AgeAtDeath = request.AgeAtDeath,
                    Notes = request.Notes,
                    Source = request.Source
                };

                var createdFamilyHistory = await _healthRecordsService.CreateFamilyHistoryAsync(familyHistory);
                
                return CreatedAtAction(nameof(GetFamilyHistoryItem), new { id = createdFamilyHistory.Id }, createdFamilyHistory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating family history");
                return BadRequest(new { message = "Failed to create family history" });
            }
        }

        [HttpPut("family-history/{id}")]
        public async Task<IActionResult> UpdateFamilyHistory(Guid id, [FromBody] FamilyHistoryRequest request)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var existingFamilyHistory = await _healthRecordsService.GetFamilyHistoryByIdAsync(id, userId);
                
                if (existingFamilyHistory == null)
                {
                    return NotFound(new { message = "Family history item not found" });
                }

                existingFamilyHistory.Condition = request.Condition ?? existingFamilyHistory.Condition;
                existingFamilyHistory.Relationship = request.Relationship ?? existingFamilyHistory.Relationship;
                existingFamilyHistory.Status = request.Status ?? existingFamilyHistory.Status;
                existingFamilyHistory.AgeAtDiagnosis = request.AgeAtDiagnosis ?? existingFamilyHistory.AgeAtDiagnosis;
                existingFamilyHistory.AgeAtDeath = request.AgeAtDeath ?? existingFamilyHistory.AgeAtDeath;
                existingFamilyHistory.Notes = request.Notes ?? existingFamilyHistory.Notes;
                existingFamilyHistory.Source = request.Source ?? existingFamilyHistory.Source;

                var updatedFamilyHistory = await _healthRecordsService.UpdateFamilyHistoryAsync(existingFamilyHistory);
                
                return Ok(updatedFamilyHistory);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Family history item not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating family history");
                return BadRequest(new { message = "Failed to update family history" });
            }
        }

        [HttpDelete("family-history/{id}")]
        public async Task<IActionResult> DeleteFamilyHistory(Guid id)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                var result = await _healthRecordsService.DeleteFamilyHistoryAsync(id, userId);
                
                if (!result)
                {
                    return NotFound(new { message = "Family history item not found" });
                }
                
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting family history");
                return BadRequest(new { message = "Failed to delete family history" });
            }
        }
        #endregion
    }

    #region Request Models
    public class MedicalConditionRequest
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Status { get; set; } // active, resolved, inactive
        public DateTime? DiagnosedDate { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public string Severity { get; set; } // mild, moderate, severe
        public string Notes { get; set; }
        public Guid? DiagnosedById { get; set; }
        public string Source { get; set; } // self-reported, provider-reported, imported
    }

    public class AllergyRequest
    {
        public string Name { get; set; }
        public string Type { get; set; } // medication, food, environmental
        public string Severity { get; set; } // mild, moderate, severe
        public string Reaction { get; set; }
        public DateTime? DiagnosedDate { get; set; }
        public string Notes { get; set; }
        public string Source { get; set; } // self-reported, provider-reported, imported
    }

    public class VitalSignRequest
    {
        public string Type { get; set; } // blood_pressure, heart_rate, respiratory_rate, temperature, weight, height, blood_glucose, oxygen_saturation
        public decimal Value { get; set; }
        public string Unit { get; set; }
        public decimal? SecondaryValue { get; set; } // For blood pressure (diastolic)
        public string SecondaryUnit { get; set; }
        public DateTime? MeasurementTime { get; set; }
        public string Source { get; set; } // self-reported, provider-reported, device, imported
        public string DeviceId { get; set; }
        public string Notes { get; set; }
    }

    public class ImmunizationRequest
    {
        public string Name { get; set; }
        public string VaccineCode { get; set; }
        public DateTime AdministrationDate { get; set; }
        public string Manufacturer { get; set; }
        public string LotNumber { get; set; }
        public string AdministeredBy { get; set; }
        public string AdministrationSite { get; set; } // left arm, right arm, etc.
        public string AdministrationRoute { get; set; } // intramuscular, subcutaneous, etc.
        public decimal? DoseQuantity { get; set; }
        public string DoseUnit { get; set; }
        public int? DoseNumber { get; set; }
        public int? TotalDoses { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string Notes { get; set; }
        public string Source { get; set; } // self-reported, provider-reported, imported
    }

    public class FamilyHistoryRequest
    {
        public string Condition { get; set; }
        public string Relationship { get; set; } // mother, father, sibling, etc.
        public string Status { get; set; } // current, deceased, etc.
        public string AgeAtDiagnosis { get; set; }
        public string AgeAtDeath { get; set; }
        public string Notes { get; set; }
        public string Source { get; set; } // self-reported, provider-reported, imported
    }
    #endregion
}

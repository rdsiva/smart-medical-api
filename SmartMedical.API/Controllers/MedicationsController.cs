using Microsoft.AspNetCore.Mvc;
using SmartMedical.Business.Interfaces;
using SmartMedical.Core.Entities.Medications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SmartMedical.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicationsController : ControllerBase
    {
        private readonly ILogger<MedicationsController> _logger;
        private readonly IMedicationService _medicationService;

        public MedicationsController(
            ILogger<MedicationsController> logger,
            IMedicationService medicationService)
        {
            _logger = logger;
            _medicationService = medicationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMedications([FromQuery] bool? active, [FromQuery] string search)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                // In a real application, this would come from the authenticated user
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
                
                var medications = await _medicationService.GetMedicationsAsync(userId, active, search);
                var response = medications.Select(m => MapToMedicationResponse(m));
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving medications");
                return BadRequest(new { message = "Failed to retrieve medications" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMedication(Guid id)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
                
                var medication = await _medicationService.GetMedicationByIdAsync(id, userId);
                
                if (medication == null)
                {
                    return NotFound(new { message = "Medication not found" });
                }
                
                return Ok(MapToMedicationResponse(medication));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving medication");
                return BadRequest(new { message = "Failed to retrieve medication" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddMedication([FromBody] AddMedicationRequest request)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
                
                var medication = new Medication
                {
                    UserId = userId,
                    Name = request.Name,
                    Dosage = request.Dosage,
                    Frequency = request.Frequency,
                    Instructions = request.Instructions,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    PrescribedBy = request.PrescribedBy,
                    Reason = request.Reason,
                    IsActive = true
                };
                
                var createdMedication = await _medicationService.CreateMedicationAsync(medication);
                
                return CreatedAtAction(
                    nameof(GetMedication), 
                    new { id = createdMedication.Id }, 
                    MapToMedicationResponse(createdMedication));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding medication");
                return BadRequest(new { message = "Failed to add medication" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedication(Guid id, [FromBody] UpdateMedicationRequest request)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
                
                var existingMedication = await _medicationService.GetMedicationByIdAsync(id, userId);
                
                if (existingMedication == null)
                {
                    return NotFound(new { message = "Medication not found" });
                }
                
                // Update medication properties
                existingMedication.Name = request.Name ?? existingMedication.Name;
                existingMedication.Dosage = request.Dosage ?? existingMedication.Dosage;
                existingMedication.Frequency = request.Frequency ?? existingMedication.Frequency;
                existingMedication.Instructions = request.Instructions ?? existingMedication.Instructions;
                existingMedication.StartDate = request.StartDate;
                existingMedication.EndDate = request.EndDate;
                existingMedication.PrescribedBy = request.PrescribedBy ?? existingMedication.PrescribedBy;
                existingMedication.Reason = request.Reason ?? existingMedication.Reason;
                existingMedication.IsActive = request.IsActive;
                
                var updatedMedication = await _medicationService.UpdateMedicationAsync(existingMedication);
                
                return Ok(MapToMedicationResponse(updatedMedication));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Medication not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating medication");
                return BadRequest(new { message = "Failed to update medication" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedication(Guid id)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
                
                var result = await _medicationService.DeleteMedicationAsync(id, userId);
                
                if (!result)
                {
                    return NotFound(new { message = "Medication not found" });
                }
                
                return Ok(new { message = "Medication deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting medication");
                return BadRequest(new { message = "Failed to delete medication" });
            }
        }

        [HttpGet("{id}/schedules")]
        public async Task<IActionResult> GetMedicationSchedules(Guid id)
        {
            try
            {
                var schedules = await _medicationService.GetMedicationSchedulesAsync(id);
                var response = schedules.Select(s => new MedicationScheduleResponse
                {
                    Id = s.Id,
                    MedicationId = s.MedicationId,
                    TimeOfDay = s.ScheduledTime.ToString("HH:mm"),
                    Dosage = s.Dosage,
                    WithFood = s.WithFood
                });
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving medication schedules");
                return BadRequest(new { message = "Failed to retrieve medication schedules" });
            }
        }

        [HttpPost("{id}/schedules")]
        public async Task<IActionResult> AddMedicationSchedule(Guid id, [FromBody] AddMedicationScheduleRequest request)
        {
            try
            {
                var schedule = new MedicationSchedule
                {
                    MedicationId = id,
                    ScheduledTime = TimeOnly.Parse(request.TimeOfDay).ToTimeSpan(),
                    Dosage = request.Dosage,
                    WithFood = request.WithFood,
                    WithWater = request.WithWater,
                    SpecialInstructions = request.SpecialInstructions,
                    ReminderEnabled = request.ReminderEnabled,
                    ReminderLeadTime = request.ReminderLeadTime
                };
                
                var createdSchedule = await _medicationService.CreateMedicationScheduleAsync(schedule);
                
                return CreatedAtAction(
                    nameof(GetMedicationSchedules), 
                    new { id = id }, 
                    new MedicationScheduleResponse
                    {
                        Id = createdSchedule.Id,
                        MedicationId = createdSchedule.MedicationId,
                        TimeOfDay = createdSchedule.ScheduledTime.ToString("HH:mm"),
                        Dosage = createdSchedule.Dosage,
                        WithFood = createdSchedule.WithFood
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding medication schedule");
                return BadRequest(new { message = "Failed to add medication schedule" });
            }
        }

        [HttpGet("schedules/{scheduleId}/doses")]
        public async Task<IActionResult> GetMedicationDoses(Guid scheduleId)
        {
            try
            {
                var doses = await _medicationService.GetMedicationDosesAsync(scheduleId);
                var response = doses.Select(d => new MedicationDoseResponse
                {
                    Id = d.Id,
                    MedicationId = d.Schedule.MedicationId,
                    ScheduledTime = d.ScheduledTime,
                    TakenTime = d.ActualTime,
                    Status = d.Taken ? "taken" : "missed",
                    DosageTaken = d.Schedule.Dosage
                });
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving medication doses");
                return BadRequest(new { message = "Failed to retrieve medication doses" });
            }
        }

        [HttpPost("schedules/{scheduleId}/doses")]
        public async Task<IActionResult> RecordMedicationDose(Guid scheduleId, [FromBody] RecordMedicationDoseRequest request)
        {
            try
            {
                var result = await _medicationService.RecordMedicationDoseAsync(
                    scheduleId, 
                    request.TakenAt, 
                    request.Taken);
                
                if (!result)
                {
                    return NotFound(new { message = "Medication schedule not found" });
                }
                
                return Ok(new { message = "Medication dose recorded successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error recording medication dose");
                return BadRequest(new { message = "Failed to record medication dose" });
            }
        }

        [HttpGet("due-for-refill")]
        public async Task<IActionResult> GetMedicationsDueForRefill([FromQuery] int daysThreshold = 7)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
                
                var medications = await _medicationService.GetMedicationsDueForRefillAsync(userId, daysThreshold);
                var response = medications.Select(m => MapToMedicationResponse(m));
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving medications due for refill");
                return BadRequest(new { message = "Failed to retrieve medications due for refill" });
            }
        }

        private MedicationResponse MapToMedicationResponse(Medication medication)
        {
            return new MedicationResponse
            {
                Id = medication.Id,
                Name = medication.Name,
                Dosage = medication.Dosage,
                Frequency = medication.Frequency,
                Instructions = medication.Instructions,
                StartDate = medication.StartDate,
                EndDate = medication.EndDate,
                PrescribedBy = medication.PrescribedBy,
                Reason = medication.Reason,
                IsActive = medication.IsActive
            };
        }
    }

    public class MedicationResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public string Instructions { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PrescribedBy { get; set; }
        public string Reason { get; set; }
        public bool IsActive { get; set; }
    }

    public class AddMedicationRequest
    {
        public string Name { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public string Instructions { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PrescribedBy { get; set; }
        public string Reason { get; set; }
    }

    public class UpdateMedicationRequest
    {
        public string Name { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public string Instructions { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PrescribedBy { get; set; }
        public string Reason { get; set; }
        public bool IsActive { get; set; }
    }

    public class MedicationScheduleResponse
    {
        public Guid Id { get; set; }
        public Guid MedicationId { get; set; }
        public string TimeOfDay { get; set; }
        public string Dosage { get; set; }
        public bool WithFood { get; set; }
    }

    public class AddMedicationScheduleRequest
    {
        public string TimeOfDay { get; set; } // Format: "HH:MM"
        public string Dosage { get; set; }
        public bool WithFood { get; set; }
        public bool WithWater { get; set; }
        public string SpecialInstructions { get; set; }
        public bool ReminderEnabled { get; set; }
        public int ReminderLeadTime { get; set; } // Minutes before scheduled time
    }

    public class MedicationDoseResponse
    {
        public Guid Id { get; set; }
        public Guid MedicationId { get; set; }
        public DateTime ScheduledTime { get; set; }
        public DateTime? TakenTime { get; set; }
        public string Status { get; set; }
        public string DosageTaken { get; set; }
    }

    public class RecordMedicationDoseRequest
    {
        public DateTime TakenAt { get; set; }
        public bool Taken { get; set; }
        public string Notes { get; set; }
    }
}

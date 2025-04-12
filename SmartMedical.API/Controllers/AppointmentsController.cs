using Microsoft.AspNetCore.Mvc;
using SmartMedical.Business.Interfaces;
using SmartMedical.Core.Entities.Appointments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SmartMedical.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly ILogger<AppointmentsController> _logger;
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(
            ILogger<AppointmentsController> logger,
            IAppointmentService appointmentService)
        {
            _logger = logger;
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAppointments(
            [FromQuery] DateTime? fromDate, 
            [FromQuery] DateTime? toDate,
            [FromQuery] string status)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                // In a real application, this would come from the authenticated user
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
                
                var appointments = await _appointmentService.GetAppointmentsAsync(userId, fromDate, toDate, status);
                var response = appointments.Select(a => MapToAppointmentResponse(a));
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving appointments");
                return BadRequest(new { message = "Failed to retrieve appointments" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointment(Guid id)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
                
                var appointment = await _appointmentService.GetAppointmentByIdAsync(id, userId);
                
                if (appointment == null)
                {
                    return NotFound(new { message = "Appointment not found" });
                }
                
                return Ok(MapToAppointmentResponse(appointment));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving appointment");
                return BadRequest(new { message = "Failed to retrieve appointment" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ScheduleAppointment([FromBody] ScheduleAppointmentRequest request)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
                
                var appointment = new Appointment
                {
                    UserId = userId,
                    ProviderId = request.ProviderId ?? Guid.Empty,
                    AppointmentType = request.AppointmentType,
                    Purpose = request.Purpose,
                    StartTime = request.StartTime,
                    EndTime = request.EndTime,
                    Location = request.Location,
                    Notes = request.Notes,
                    Status = "scheduled"
                };
                
                var createdAppointment = await _appointmentService.CreateAppointmentAsync(appointment);
                
                return CreatedAtAction(
                    nameof(GetAppointment), 
                    new { id = createdAppointment.Id }, 
                    MapToAppointmentResponse(createdAppointment));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error scheduling appointment");
                return BadRequest(new { message = "Failed to schedule appointment" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(Guid id, [FromBody] UpdateAppointmentRequest request)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
                
                var existingAppointment = await _appointmentService.GetAppointmentByIdAsync(id, userId);
                
                if (existingAppointment == null)
                {
                    return NotFound(new { message = "Appointment not found" });
                }
                
                // Update appointment properties
                existingAppointment.AppointmentType = request.AppointmentType ?? existingAppointment.AppointmentType;
                existingAppointment.Purpose = request.Purpose ?? existingAppointment.Purpose;
                existingAppointment.StartTime = request.StartTime;
                existingAppointment.EndTime = request.EndTime;
                existingAppointment.Location = request.Location ?? existingAppointment.Location;
                existingAppointment.Notes = request.Notes ?? existingAppointment.Notes;
                existingAppointment.Status = request.Status ?? existingAppointment.Status;
                
                if (request.ProviderId.HasValue && request.ProviderId.Value != Guid.Empty)
                {
                    existingAppointment.ProviderId = request.ProviderId.Value;
                }
                
                var updatedAppointment = await _appointmentService.UpdateAppointmentAsync(existingAppointment);
                
                return Ok(MapToAppointmentResponse(updatedAppointment));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Appointment not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating appointment");
                return BadRequest(new { message = "Failed to update appointment" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelAppointment(Guid id)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
                
                var result = await _appointmentService.CancelAppointmentAsync(id, userId);
                
                if (!result)
                {
                    return NotFound(new { message = "Appointment not found" });
                }
                
                return Ok(new { message = "Appointment cancelled successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling appointment");
                return BadRequest(new { message = "Failed to cancel appointment" });
            }
        }

        [HttpGet("{id}/reminders")]
        public async Task<IActionResult> GetAppointmentReminders(Guid id)
        {
            try
            {
                var reminders = await _appointmentService.GetAppointmentRemindersAsync(id);
                var response = reminders.Select(r => new AppointmentReminderResponse
                {
                    Id = r.Id,
                    AppointmentId = r.AppointmentId,
                    ReminderTime = r.ReminderTime,
                    IsSent = r.IsSent,
                    IsAcknowledged = r.IsAcknowledged,
                    AcknowledgedTime = r.AcknowledgedTime
                });
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving appointment reminders");
                return BadRequest(new { message = "Failed to retrieve appointment reminders" });
            }
        }

        [HttpPost("{id}/check-in")]
        public async Task<IActionResult> CheckInForAppointment(Guid id)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
                
                var result = await _appointmentService.CheckInForAppointmentAsync(id, userId);
                
                if (!result)
                {
                    return BadRequest(new { message = "Unable to check in for appointment" });
                }
                
                return Ok(new { message = "Check-in successful" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking in for appointment");
                return BadRequest(new { message = "Failed to check in for appointment" });
            }
        }

        [HttpGet("upcoming")]
        public async Task<IActionResult> GetUpcomingAppointments([FromQuery] int days = 7)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
                
                var appointments = await _appointmentService.GetUpcomingAppointmentsAsync(userId, days);
                var response = appointments.Select(a => MapToAppointmentResponse(a));
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving upcoming appointments");
                return BadRequest(new { message = "Failed to retrieve upcoming appointments" });
            }
        }

        private AppointmentResponse MapToAppointmentResponse(Appointment appointment)
        {
            return new AppointmentResponse
            {
                Id = appointment.Id,
                AppointmentType = appointment.AppointmentType,
                Purpose = appointment.Purpose,
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime,
                Location = appointment.Location,
                Notes = appointment.Notes,
                Status = appointment.Status,
                ProviderName = appointment.Provider?.Provider?.Name ?? "Unknown Provider"
            };
        }
    }

    public class AppointmentResponse
    {
        public Guid Id { get; set; }
        public string AppointmentType { get; set; }
        public string Purpose { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Location { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
        public string ProviderName { get; set; }
    }

    public class ScheduleAppointmentRequest
    {
        public string AppointmentType { get; set; }
        public string Purpose { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Location { get; set; }
        public string Notes { get; set; }
        public Guid? ProviderId { get; set; }
    }

    public class UpdateAppointmentRequest
    {
        public string AppointmentType { get; set; }
        public string Purpose { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Location { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
        public Guid? ProviderId { get; set; }
    }

    public class AppointmentReminderResponse
    {
        public Guid Id { get; set; }
        public Guid AppointmentId { get; set; }
        public DateTime ReminderTime { get; set; }
        public bool IsSent { get; set; }
        public bool IsAcknowledged { get; set; }
        public DateTime? AcknowledgedTime { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartMedical.Business.Interfaces;
using SmartMedical.Core.Entities.Appointments;
using SmartMedical.Core.Interfaces;

namespace SmartMedical.Business.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsAsync(Guid userId, DateTime? fromDate = null, DateTime? toDate = null, string status = null)
        {
            return await _appointmentRepository.GetAppointmentsAsync(userId, fromDate, toDate, status);
        }

        public async Task<Appointment> GetAppointmentByIdAsync(Guid id, Guid userId)
        {
            return await _appointmentRepository.GetAppointmentByIdAsync(id, userId);
        }

        public async Task<Appointment> CreateAppointmentAsync(Appointment appointment)
        {
            // Set default values if not provided
            if (string.IsNullOrEmpty(appointment.Status))
            {
                appointment.Status = "scheduled";
            }

            return await _appointmentRepository.CreateAppointmentAsync(appointment);
        }

        public async Task<Appointment> UpdateAppointmentAsync(Appointment appointment)
        {
            var existingAppointment = await _appointmentRepository.GetAppointmentByIdAsync(appointment.Id, appointment.UserId);
            if (existingAppointment == null)
            {
                throw new KeyNotFoundException($"Appointment with ID {appointment.Id} not found");
            }

            return await _appointmentRepository.UpdateAppointmentAsync(appointment);
        }

        public async Task<bool> CancelAppointmentAsync(Guid id, Guid userId)
        {
            var appointment = await _appointmentRepository.GetAppointmentByIdAsync(id, userId);
            if (appointment == null)
            {
                return false;
            }

            appointment.Status = "cancelled";
            appointment.UpdatedAt = DateTime.UtcNow;

            await _appointmentRepository.UpdateAppointmentAsync(appointment);
            return true;
        }

        public async Task<IEnumerable<AppointmentReminder>> GetAppointmentRemindersAsync(Guid appointmentId)
        {
            return await _appointmentRepository.GetAppointmentRemindersAsync(appointmentId);
        }

        public async Task<AppointmentReminder> CreateAppointmentReminderAsync(AppointmentReminder reminder)
        {
            return await _appointmentRepository.CreateAppointmentReminderAsync(reminder);
        }

        public async Task<AppointmentReminder> UpdateAppointmentReminderAsync(AppointmentReminder reminder)
        {
            return await _appointmentRepository.UpdateAppointmentReminderAsync(reminder);
        }

        public async Task<bool> DeleteAppointmentReminderAsync(Guid id)
        {
            return await _appointmentRepository.DeleteAppointmentReminderAsync(id);
        }

        public async Task<IEnumerable<Appointment>> GetUpcomingAppointmentsAsync(Guid userId, int days)
        {
            return await _appointmentRepository.GetUpcomingAppointmentsAsync(userId, days);
        }

        public async Task<bool> CheckInForAppointmentAsync(Guid id, Guid userId)
        {
            var appointment = await _appointmentRepository.GetAppointmentByIdAsync(id, userId);
            if (appointment == null)
            {
                return false;
            }

            // Only allow check-in if the appointment is scheduled or confirmed
            if (appointment.Status != "scheduled" && appointment.Status != "confirmed")
            {
                return false;
            }

            appointment.Status = "checked_in";
            appointment.UpdatedAt = DateTime.UtcNow;

            await _appointmentRepository.UpdateAppointmentAsync(appointment);
            return true;
        }
    }
}

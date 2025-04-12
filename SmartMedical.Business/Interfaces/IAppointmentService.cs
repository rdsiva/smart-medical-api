using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartMedical.Core.Entities.Appointments;

namespace SmartMedical.Business.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<Appointment>> GetAppointmentsAsync(Guid userId, DateTime? fromDate = null, DateTime? toDate = null, string status = null);
        Task<Appointment> GetAppointmentByIdAsync(Guid id, Guid userId);
        Task<Appointment> CreateAppointmentAsync(Appointment appointment);
        Task<Appointment> UpdateAppointmentAsync(Appointment appointment);
        Task<bool> CancelAppointmentAsync(Guid id, Guid userId);
        
        // Appointment Reminders
        Task<IEnumerable<AppointmentReminder>> GetAppointmentRemindersAsync(Guid appointmentId);
        Task<AppointmentReminder> CreateAppointmentReminderAsync(AppointmentReminder reminder);
        Task<AppointmentReminder> UpdateAppointmentReminderAsync(AppointmentReminder reminder);
        Task<bool> DeleteAppointmentReminderAsync(Guid id);
        
        // Additional methods for appointment management
        Task<IEnumerable<Appointment>> GetUpcomingAppointmentsAsync(Guid userId, int days);
        Task<bool> CheckInForAppointmentAsync(Guid id, Guid userId);
    }
}

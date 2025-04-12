using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartMedical.Core.Entities.Appointments;
using SmartMedical.Core.Interfaces;
using SmartMedical.Infrastructure.Data;

namespace SmartMedical.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ApplicationDbContext _context;

        public AppointmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsAsync(Guid userId, DateTime? fromDate = null, DateTime? toDate = null, string status = null)
        {
            var query = _context.Appointments
                .Include(a => a.Provider)
                .ThenInclude(p => p.Provider)
                .Where(a => a.UserId == userId);

            if (fromDate.HasValue)
            {
                query = query.Where(a => a.StartTime >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(a => a.StartTime <= toDate.Value);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(a => a.Status == status);
            }

            return await query.OrderBy(a => a.StartTime).ToListAsync();
        }

        public async Task<Appointment> GetAppointmentByIdAsync(Guid id, Guid userId)
        {
            return await _context.Appointments
                .Include(a => a.Provider)
                .ThenInclude(p => p.Provider)
                .Include(a => a.Reminders)
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);
        }

        public async Task<Appointment> CreateAppointmentAsync(Appointment appointment)
        {
            appointment.Id = Guid.NewGuid();
            appointment.CreatedAt = DateTime.UtcNow;
            appointment.UpdatedAt = DateTime.UtcNow;
            
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            
            return appointment;
        }

        public async Task<Appointment> UpdateAppointmentAsync(Appointment appointment)
        {
            appointment.UpdatedAt = DateTime.UtcNow;
            
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
            
            return appointment;
        }

        public async Task<bool> DeleteAppointmentAsync(Guid id, Guid userId)
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);
                
            if (appointment == null)
            {
                return false;
            }
            
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<IEnumerable<AppointmentReminder>> GetAppointmentRemindersAsync(Guid appointmentId)
        {
            return await _context.AppointmentReminders
                .Where(r => r.AppointmentId == appointmentId)
                .OrderBy(r => r.ReminderTime)
                .ToListAsync();
        }

        public async Task<AppointmentReminder> CreateAppointmentReminderAsync(AppointmentReminder reminder)
        {
            reminder.Id = Guid.NewGuid();
            reminder.CreatedAt = DateTime.UtcNow;
            reminder.UpdatedAt = DateTime.UtcNow;
            
            _context.AppointmentReminders.Add(reminder);
            await _context.SaveChangesAsync();
            
            return reminder;
        }

        public async Task<AppointmentReminder> UpdateAppointmentReminderAsync(AppointmentReminder reminder)
        {
            reminder.UpdatedAt = DateTime.UtcNow;
            
            _context.AppointmentReminders.Update(reminder);
            await _context.SaveChangesAsync();
            
            return reminder;
        }

        public async Task<bool> DeleteAppointmentReminderAsync(Guid id)
        {
            var reminder = await _context.AppointmentReminders
                .FirstOrDefaultAsync(r => r.Id == id);
                
            if (reminder == null)
            {
                return false;
            }
            
            _context.AppointmentReminders.Remove(reminder);
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<IEnumerable<Appointment>> GetUpcomingAppointmentsAsync(Guid userId, int days)
        {
            var endDate = DateTime.UtcNow.AddDays(days);
            
            return await _context.Appointments
                .Include(a => a.Provider)
                .ThenInclude(p => p.Provider)
                .Include(a => a.Reminders)
                .Where(a => a.UserId == userId && 
                       a.StartTime >= DateTime.UtcNow && 
                       a.StartTime <= endDate &&
                       a.Status != "cancelled")
                .OrderBy(a => a.StartTime)
                .ToListAsync();
        }
    }
}

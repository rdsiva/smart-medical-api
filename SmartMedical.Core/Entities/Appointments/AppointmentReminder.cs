using System;

namespace SmartMedical.Core.Entities.Appointments
{
    public class AppointmentReminder
    {
        public Guid Id { get; set; }
        public Guid AppointmentId { get; set; }
        public DateTime ReminderTime { get; set; }
        public bool IsSent { get; set; }
        public bool IsAcknowledged { get; set; }
        public DateTime? AcknowledgedTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public Appointment Appointment { get; set; }
    }
}

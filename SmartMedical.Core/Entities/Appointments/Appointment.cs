using System;
using System.Collections.Generic;

namespace SmartMedical.Core.Entities.Appointments
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid? ProviderId { get; set; }
        public string AppointmentType { get; set; }
        public string Purpose { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Location { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; } // 'scheduled', 'confirmed', 'cancelled', 'completed'
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public Auth.User User { get; set; }
        public ICollection<AppointmentReminder> Reminders { get; set; }
    }
}

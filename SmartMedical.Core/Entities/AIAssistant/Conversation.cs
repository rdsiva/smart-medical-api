using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartMedical.Core.Entities.AIAssistant
{
    public class Conversation
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public Auth.User User { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}

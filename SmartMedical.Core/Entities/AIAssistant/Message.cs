using System;

namespace SmartMedical.Core.Entities.AIAssistant
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
        public string Role { get; set; } // 'user', 'assistant', 'system'
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public int? TokensUsed { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public Conversation Conversation { get; set; }
    }
}

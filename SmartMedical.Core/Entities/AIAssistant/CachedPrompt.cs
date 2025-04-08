using System;

namespace SmartMedical.Core.Entities.AIAssistant
{
    public class CachedPrompt
    {
        public Guid Id { get; set; }
        public string PromptHash { get; set; }
        public string PromptText { get; set; }
        public string ResponseText { get; set; }
        public string ModelUsed { get; set; }
        public int TokensUsed { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUsedAt { get; set; }
        public int UseCount { get; set; }
    }
}

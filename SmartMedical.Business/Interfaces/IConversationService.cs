using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartMedical.Core.Entities.AIAssistant;

namespace SmartMedical.Business.Interfaces
{
    public interface IConversationService
    {
        Task<IEnumerable<Conversation>> GetConversationsAsync(Guid userId, string search = null);
        Task<Conversation> GetConversationByIdAsync(Guid id, Guid userId);
        Task<Conversation> CreateConversationAsync(Conversation conversation);
        Task<Conversation> UpdateConversationAsync(Conversation conversation);
        Task<bool> DeleteConversationAsync(Guid id, Guid userId);
        
        // Messages
        Task<IEnumerable<Message>> GetMessagesAsync(Guid conversationId, int limit = 50, int offset = 0);
        Task<Message> CreateUserMessageAsync(Guid conversationId, string content);
        Task<Message> CreateAssistantMessageAsync(Guid conversationId, string content);
        
        // Health questions and symptom assessment
        Task<(string answer, IEnumerable<string> sources)> AskHealthQuestionAsync(Guid userId, string question, bool includePersonalContext = true);
        Task<(string assessment, IEnumerable<string> recommendations)> PerformSymptomAssessmentAsync(Guid userId, string primarySymptom, IEnumerable<string> additionalSymptoms, string symptomDuration, string symptomSeverity);
    }
}

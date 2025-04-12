using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartMedical.Core.Entities.AIAssistant;

namespace SmartMedical.Core.Interfaces
{
    public interface IConversationRepository
    {
        Task<IEnumerable<Conversation>> GetConversationsAsync(Guid userId, string search = null);
        Task<Conversation> GetConversationByIdAsync(Guid id, Guid userId);
        Task<Conversation> CreateConversationAsync(Conversation conversation);
        Task<Conversation> UpdateConversationAsync(Conversation conversation);
        Task<bool> DeleteConversationAsync(Guid id, Guid userId);
        
        // Messages
        Task<IEnumerable<Message>> GetMessagesAsync(Guid conversationId, int limit = 50, int offset = 0);
        Task<Message> GetMessageByIdAsync(Guid id);
        Task<Message> CreateMessageAsync(Message message);
        Task<Message> CreateAssistantMessageAsync(Message message);
        
        // Additional methods
        Task<Conversation> GetLatestConversationAsync(Guid userId);
    }
}

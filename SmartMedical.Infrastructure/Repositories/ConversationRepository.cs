using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartMedical.Core.Entities.AIAssistant;
using SmartMedical.Core.Interfaces;
using SmartMedical.Infrastructure.Data;

namespace SmartMedical.Infrastructure.Repositories
{
    public class ConversationRepository : IConversationRepository
    {
        private readonly ApplicationDbContext _context;

        public ConversationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Conversation>> GetConversationsAsync(Guid userId, string search = null)
        {
            var query = _context.Conversations
                .Where(c => c.UserId == userId);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.Title.Contains(search) || 
                                         c.Messages.Any(m => m.Content.Contains(search)));
            }

            return await query.OrderByDescending(c => c.UpdatedAt).ToListAsync();
        }

        public async Task<Conversation> GetConversationByIdAsync(Guid id, Guid userId)
        {
            return await _context.Conversations
                .Include(c => c.Messages.OrderBy(m => m.Timestamp))
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
        }

        public async Task<Conversation> CreateConversationAsync(Conversation conversation)
        {
            conversation.Id = Guid.NewGuid();
            conversation.CreatedAt = DateTime.UtcNow;
            conversation.UpdatedAt = DateTime.UtcNow;
            conversation.StartTime = DateTime.UtcNow;
            conversation.IsActive = true;
            
            _context.Conversations.Add(conversation);
            await _context.SaveChangesAsync();
            
            return conversation;
        }

        public async Task<Conversation> UpdateConversationAsync(Conversation conversation)
        {
            conversation.UpdatedAt = DateTime.UtcNow;
            
            _context.Conversations.Update(conversation);
            await _context.SaveChangesAsync();
            
            return conversation;
        }

        public async Task<bool> DeleteConversationAsync(Guid id, Guid userId)
        {
            var conversation = await _context.Conversations
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
                
            if (conversation == null)
            {
                return false;
            }
            
            _context.Conversations.Remove(conversation);
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync(Guid conversationId, int limit = 50, int offset = 0)
        {
            return await _context.Messages
                .Where(m => m.ConversationId == conversationId)
                .OrderBy(m => m.Timestamp)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<Message> GetMessageByIdAsync(Guid id)
        {
            return await _context.Messages
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Message> CreateMessageAsync(Message message)
        {
            message.Id = Guid.NewGuid();
            message.CreatedAt = DateTime.UtcNow;
            message.Timestamp = DateTime.UtcNow;
            
            _context.Messages.Add(message);
            
            // Update the conversation's UpdatedAt timestamp
            var conversation = await _context.Conversations.FindAsync(message.ConversationId);
            if (conversation != null)
            {
                conversation.UpdatedAt = DateTime.UtcNow;
                _context.Conversations.Update(conversation);
            }
            
            await _context.SaveChangesAsync();
            
            return message;
        }

        public async Task<Message> CreateAssistantMessageAsync(Message message)
        {
            // Same implementation as CreateMessageAsync, but kept separate for potential future differences
            return await CreateMessageAsync(message);
        }

        public async Task<Conversation> GetLatestConversationAsync(Guid userId)
        {
            return await _context.Conversations
                .Where(c => c.UserId == userId && c.IsActive)
                .OrderByDescending(c => c.UpdatedAt)
                .FirstOrDefaultAsync();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using SmartMedical.Business.Interfaces;
using SmartMedical.Core.Entities.AIAssistant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SmartMedical.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AIAssistantController : ControllerBase
    {
        private readonly ILogger<AIAssistantController> _logger;
        private readonly IConversationService _conversationService;

        public AIAssistantController(
            ILogger<AIAssistantController> logger,
            IConversationService conversationService)
        {
            _logger = logger;
            _conversationService = conversationService;
        }

        [HttpGet("conversations")]
        public async Task<IActionResult> GetConversations([FromQuery] string search)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                // In a real application, this would come from the authenticated user
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
                
                var conversations = await _conversationService.GetConversationsAsync(userId, search);
                var response = conversations.Select(c => new ConversationResponse
                {
                    Id = c.Id,
                    Title = c.Title,
                    StartTime = c.StartTime,
                    EndTime = c.EndTime,
                    IsActive = c.IsActive
                });
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving conversations");
                return BadRequest(new { message = "Failed to retrieve conversations" });
            }
        }

        [HttpGet("conversations/{id}")]
        public async Task<IActionResult> GetConversation(Guid id)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
                
                var conversation = await _conversationService.GetConversationByIdAsync(id, userId);
                
                if (conversation == null)
                {
                    return NotFound(new { message = "Conversation not found" });
                }
                
                var response = new ConversationResponse
                {
                    Id = conversation.Id,
                    Title = conversation.Title,
                    StartTime = conversation.StartTime,
                    EndTime = conversation.EndTime,
                    IsActive = conversation.IsActive
                };
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving conversation");
                return BadRequest(new { message = "Failed to retrieve conversation" });
            }
        }

        [HttpPost("conversations")]
        public async Task<IActionResult> StartConversation([FromBody] StartConversationRequest request)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
                
                var conversation = new Conversation
                {
                    UserId = userId,
                    Title = request.Title
                };
                
                var createdConversation = await _conversationService.CreateConversationAsync(conversation);
                
                // If an initial message was provided, add it to the conversation
                if (!string.IsNullOrEmpty(request.InitialMessage))
                {
                    await _conversationService.CreateUserMessageAsync(createdConversation.Id, request.InitialMessage);
                    
                    // Generate an assistant response
                    var assistantResponse = "Thank you for your message. How can I assist you with your health concerns today?";
                    await _conversationService.CreateAssistantMessageAsync(createdConversation.Id, assistantResponse);
                }
                
                return CreatedAtAction(
                    nameof(GetConversation), 
                    new { id = createdConversation.Id }, 
                    new ConversationResponse
                    {
                        Id = createdConversation.Id,
                        Title = createdConversation.Title,
                        StartTime = createdConversation.StartTime,
                        EndTime = createdConversation.EndTime,
                        IsActive = createdConversation.IsActive
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting conversation");
                return BadRequest(new { message = "Failed to start conversation" });
            }
        }

        [HttpPut("conversations/{id}")]
        public async Task<IActionResult> UpdateConversation(Guid id, [FromBody] UpdateConversationRequest request)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
                
                var conversation = await _conversationService.GetConversationByIdAsync(id, userId);
                
                if (conversation == null)
                {
                    return NotFound(new { message = "Conversation not found" });
                }
                
                // Update conversation properties
                conversation.Title = request.Title ?? conversation.Title;
                conversation.IsActive = request.IsActive;
                
                if (!request.IsActive && !conversation.EndTime.HasValue)
                {
                    conversation.EndTime = DateTime.UtcNow;
                }
                
                var updatedConversation = await _conversationService.UpdateConversationAsync(conversation);
                
                return Ok(new ConversationResponse
                {
                    Id = updatedConversation.Id,
                    Title = updatedConversation.Title,
                    StartTime = updatedConversation.StartTime,
                    EndTime = updatedConversation.EndTime,
                    IsActive = updatedConversation.IsActive
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Conversation not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating conversation");
                return BadRequest(new { message = "Failed to update conversation" });
            }
        }

        [HttpDelete("conversations/{id}")]
        public async Task<IActionResult> DeleteConversation(Guid id)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
                
                var result = await _conversationService.DeleteConversationAsync(id, userId);
                
                if (!result)
                {
                    return NotFound(new { message = "Conversation not found" });
                }
                
                return Ok(new { message = "Conversation deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting conversation");
                return BadRequest(new { message = "Failed to delete conversation" });
            }
        }

        [HttpGet("conversations/{id}/messages")]
        public async Task<IActionResult> GetMessages(Guid id, [FromQuery] int limit = 50, [FromQuery] int offset = 0)
        {
            try
            {
                var messages = await _conversationService.GetMessagesAsync(id, limit, offset);
                var response = messages.Select(m => new MessageResponse
                {
                    Id = m.Id,
                    ConversationId = m.ConversationId,
                    Role = m.Role,
                    Content = m.Content,
                    Timestamp = m.Timestamp
                });
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving messages");
                return BadRequest(new { message = "Failed to retrieve messages" });
            }
        }

        [HttpPost("conversations/{id}/messages")]
        public async Task<IActionResult> SendMessage(Guid id, [FromBody] SendMessageRequest request)
        {
            try
            {
                // Create user message
                var userMessage = await _conversationService.CreateUserMessageAsync(id, request.Content);
                
                // Generate assistant response
                // In a real implementation, this would call an AI service
                var assistantResponse = "Thank you for your message. This is a placeholder response. In a real implementation, this would be generated by an AI service based on your message content and conversation history.";
                var assistantMessage = await _conversationService.CreateAssistantMessageAsync(id, assistantResponse);
                
                return Ok(new
                {
                    userMessage = new MessageResponse
                    {
                        Id = userMessage.Id,
                        ConversationId = userMessage.ConversationId,
                        Role = userMessage.Role,
                        Content = userMessage.Content,
                        Timestamp = userMessage.Timestamp
                    },
                    assistantMessage = new MessageResponse
                    {
                        Id = assistantMessage.Id,
                        ConversationId = assistantMessage.ConversationId,
                        Role = assistantMessage.Role,
                        Content = assistantMessage.Content,
                        Timestamp = assistantMessage.Timestamp
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message");
                return BadRequest(new { message = "Failed to send message" });
            }
        }

        [HttpPost("health-question")]
        public async Task<IActionResult> AskHealthQuestion([FromBody] HealthQuestionRequest request)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
                
                var (answer, sources) = await _conversationService.AskHealthQuestionAsync(
                    userId, 
                    request.Question, 
                    request.IncludePersonalContext);
                
                return Ok(new
                {
                    answer = answer,
                    sources = sources,
                    disclaimer = "This information is for educational purposes only and is not a substitute for professional medical advice. Always consult with a qualified healthcare provider for medical advice."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing health question");
                return BadRequest(new { message = "Failed to process health question" });
            }
        }

        [HttpPost("symptom-assessment")]
        public async Task<IActionResult> PerformSymptomAssessment([FromBody] SymptomAssessmentRequest request)
        {
            try
            {
                // For demo purposes, using a hardcoded user ID
                var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
                
                var (assessment, recommendations) = await _conversationService.PerformSymptomAssessmentAsync(
                    userId,
                    request.PrimarySymptom,
                    request.AdditionalSymptoms,
                    request.SymptomDuration,
                    request.SymptomSeverity);
                
                return Ok(new
                {
                    assessment = assessment,
                    recommendations = recommendations,
                    disclaimer = "This symptom assessment is for informational purposes only and does not constitute medical advice. It is not a substitute for professional medical advice, diagnosis, or treatment. Always seek the advice of your physician or other qualified health provider with any questions you may have regarding a medical condition."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error performing symptom assessment");
                return BadRequest(new { message = "Failed to perform symptom assessment" });
            }
        }
    }

    public class ConversationResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsActive { get; set; }
    }

    public class StartConversationRequest
    {
        public string Title { get; set; }
        public string InitialMessage { get; set; }
    }

    public class UpdateConversationRequest
    {
        public string Title { get; set; }
        public bool IsActive { get; set; }
    }

    public class MessageResponse
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
        public string Role { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class SendMessageRequest
    {
        public string Content { get; set; }
    }

    public class HealthQuestionRequest
    {
        public string Question { get; set; }
        public bool IncludePersonalContext { get; set; } = true;
    }

    public class SymptomAssessmentRequest
    {
        public string PrimarySymptom { get; set; }
        public List<string> AdditionalSymptoms { get; set; }
        public string SymptomDuration { get; set; }
        public string SymptomSeverity { get; set; }
    }
}

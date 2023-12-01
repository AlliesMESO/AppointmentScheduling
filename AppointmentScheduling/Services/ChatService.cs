using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // Include Entity Framework Core if needed
using AppointmentScheduling.Models.ViewModels;
using AppointmentScheduling.Models;

namespace AppointmentScheduling.Services
{
    public class ChatService : IChatService
    {
        private readonly ApplicationDbContext _db;

        public ChatService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task SendMessageAsync(ChatMessage message)
        {
            // Implement the logic to send and save the chat message to the database

            _db.Chats.Add(message);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<ChatMessage>> GetChatHistoryAsync(string senderId, string recipientId)
        {
            // Implement the logic to retrieve chat history between sender and recipient

            var chatHistory = await _db.Chats
            .Where(c => (c.SenderId == senderId && c.RecipientId == recipientId) ||
                        (c.SenderId == recipientId && c.RecipientId == senderId))
            .OrderBy(c => c.Timestamp)
            .ToListAsync();

            return chatHistory;
        }
    } 
}

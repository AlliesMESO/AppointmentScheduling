using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppointmentScheduling.Models.ViewModels;

namespace AppointmentScheduling.Services
{
    public interface IChatService
    {
        Task SendMessageAsync(ChatMessage message);
        Task<IEnumerable<ChatMessage>> GetChatHistoryAsync(string senderId, string recipientId);

    }
}

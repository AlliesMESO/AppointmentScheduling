using AppointmentScheduling.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AppointmentScheduling.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {

            List<ChatMessage> messages = GetMessages();

            return View(messages);
        }

     private List<ChatMessage> GetMessages()
        {
            List<ChatMessage> messages = new List<ChatMessage>
            {
                new ChatMessage { SenderId = "sender1", RecipientId = "recipient1", Content = "Hello!"},
                new ChatMessage { SenderId = "recipient1", RecipientId = "sender1", Content = "Hi there"},
            };

            return messages;
        }

        public IActionResult Chat()
        {
            var chatMessage = new ChatMessage
            {
                SenderName = "DefaultSender"
            };

            return View(chatMessage);
        }
    }
}

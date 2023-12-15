using AppointmentScheduling.Models;
using AppointmentScheduling.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AppointmentScheduling.Controllers
{
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ChatController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {

            // Get the list of registered users
            List<ApplicationUser> registeredUsers = GetRegisteredUsers();
            return View(registeredUsers);
            //List<ChatMessage> messages = GetMessages();

            //return View(messages);
        }


        private List<ApplicationUser> GetRegisteredUsers()
        {
            if (_dbContext != null)
            {
                // Logic to retrieve registered users from your data store
                // For example, you might use your ApplicationUserManager to get users
                List<ApplicationUser> registeredUsers = _dbContext.Users.AsNoTracking().ToList();

                return registeredUsers;
            }

            // Handle the case where _dbContext is null
            return new List<ApplicationUser>();
        }

        private List<ChatMessage> GetChatMessages()
        {
            List<ChatMessage> chatMessages = new List<ChatMessage>
            {
                new ChatMessage { SenderId = "sender1", RecipientId = "recipient1", Content = "Hello!"},
                new ChatMessage { SenderId = "recipient1", RecipientId = "sender1", Content = "Hi there"},
            };

            return chatMessages;
        }

        public IActionResult Chat()
        {
            List<ApplicationUser> registeredUsers = GetRegisteredUsers();

            return View(registeredUsers);
        }
    }
}

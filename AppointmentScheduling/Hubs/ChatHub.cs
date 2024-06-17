using AppointmentScheduling.Utility;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace AppointmentScheduling.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IEmailSenders _emailSenders;
        public ChatHub(IEmailSenders emailSenders)
        {
            _emailSenders = emailSenders;
        }

        public async Task SendMessage(string user,string message, string recipientId)
        {
            try
            {
                var userName = Context.Items["UserName"] as string;
                Console.WriteLine($"Received message from {userName}: {message}");
                
                // Send the message only to the specific client with the provided recipientId

                await Clients.Client(recipientId).SendAsync("MessageSent", userName, message);
            }
            catch (Exception ex) 
            {
                // Log the exception

                Console.Error.WriteLine($"Error in SendMessage: {ex}");

                await Clients.Client(Context.ConnectionId).SendAsync("SendMessageError", ex.Message);
            }
        }

        public async Task SendEmailMessage(string user, string message)
        {
            try
            {
                //await new EmailSender(configuration).SendEmailAsync(recipientId, user, message);

                // Broadcast the email message only to the specified user

                await Clients.All.SendAsync("RecieveEmailMessage", user, message);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in Send Email Message: {ex.Message}");

                //await Clients.Client(Context.ConnectionId).SendAsync("SendEmailMessageError", ex.Message);
            }
        }
        public override Task OnConnectedAsync()
        {
            // Get the user's name from your authentication system
            var userName = Context.User.Identity.Name;

            // Store the user's name in the Context.Items collection
            Context.Items["UserName"] = userName;

            return base.OnConnectedAsync();
        }
    }
}

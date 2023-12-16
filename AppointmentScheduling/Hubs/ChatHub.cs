using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace AppointmentScheduling.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user,string message)
        {
            var userName = Context.Items["UserName"] as string;
            Console.WriteLine($"Received message from {userName}: {message}");
            await Clients.All.SendAsync("ReceiveMessage", userName, message);
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

using Azure.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace AppointmentScheduling.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message) //the SendMessage method is responsible for receiving a message from a client and broadcasting it to all connected clients
        {
            var userName = Context.Items["UserName"] as string;
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public override Task OnConnectedAsync ()
        {
            // Get the user's name from your authentication system
            var userName = Context.User.Identity.Name;

            // Store the user's name in the Context.Items collection
            Context.Items["UserName"] = userName;

            return base.OnConnectedAsync ();
        }

  
    }
}

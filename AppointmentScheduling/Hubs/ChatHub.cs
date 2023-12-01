using Azure.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace AppointmentScheduling.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message) //the SendMessage method is responsible for receiving a message from a client and broadcasting it to all connected clients
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}

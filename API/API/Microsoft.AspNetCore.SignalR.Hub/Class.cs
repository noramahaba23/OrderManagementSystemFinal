using Microsoft.AspNetCore.SignalR;

namespace API.Hub
{
    public class Class : Microsoft.AspNetCore.SignalR.Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}

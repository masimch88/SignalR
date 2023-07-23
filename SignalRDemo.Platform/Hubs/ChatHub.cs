using Microsoft.AspNetCore.SignalR;

namespace SignalRDemo.Platform.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage()
        {
            await Clients.All.SendAsync("ReceiveMessage", "Test Message");
        }        
    }
}

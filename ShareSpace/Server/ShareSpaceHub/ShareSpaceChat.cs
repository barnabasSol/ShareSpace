using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ShareSpace.Server.ShareSpaceHub
{
    public class ShareSpaceChat : Hub
    {
        [Authorize]
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}

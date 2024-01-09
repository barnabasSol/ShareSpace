using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ShareSpace.Server.ShareSpaceHub;

public class NotificationHub : Hub
{
    [Authorize]
    public async Task NotifyUserMessage(string username, string message)
    {
        await Clients
            .User(username)
            .SendAsync("ReceiveNotificationFromUser", Context.UserIdentifier, message);
    }
}

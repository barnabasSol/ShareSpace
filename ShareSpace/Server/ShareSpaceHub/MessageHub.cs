using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ShareSpace.Server.Data;
using ShareSpace.Server.Repository.Contracts;
using ShareSpace.Shared.DTOs;

namespace ShareSpace.Server.ShareSpaceHub;

[Authorize(Roles = "user")]
public class MessageHub : Hub
{
    private readonly IMessageRepository messageRepository;
    private readonly ShareSpaceDbContext shareSpaceDb;

    public MessageHub(IMessageRepository messageRepository, ShareSpaceDbContext shareSpaceDb)
    {
        this.messageRepository = messageRepository;
        this.shareSpaceDb = shareSpaceDb;
    }

    public override async Task OnConnectedAsync()
    {
        Guid current_user = Guid.Parse(
            Context.User!.Claims.FirstOrDefault(w => w.Type == "Sub")!.Value
        );
        var connected_user = await shareSpaceDb.Users.FindAsync(current_user);
        if (connected_user is not null)
        {
            connected_user.OnlineStatus = true;
            await shareSpaceDb.SaveChangesAsync();
            await Clients.All.SendAsync("UserOnlineStatusChanged", current_user, true);
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Guid current_user = Guid.Parse(
            Context.User!.Claims.FirstOrDefault(w => w.Type == "Sub")!.Value
        );
        var connected_user = await shareSpaceDb.Users.FindAsync(current_user);
        if (connected_user is not null)
        {
            connected_user.OnlineStatus = false;
            await shareSpaceDb.SaveChangesAsync();
            await Clients.All.SendAsync("UserOnlineStatusChanged", current_user, true);
        }
    }

    public async Task FetchMessagesOfUser(string username)
    {
        Guid current_user = Guid.Parse(
            Context.User!.Claims.Where(w => w.Type == "Sub").Select(s => s.Value).FirstOrDefault()!
        );
        var response = await messageRepository.GetMessagesOfUser(
            current_user,
            other_user: username
        );

        if (response.IsSuccess)
            await Clients.Caller.SendAsync("ShowMessages", response.Data);
    }

    public async Task GetUnseenMessagesCount()
    {
        Guid current_user = Guid.Parse(
            Context.User!.Claims.Where(w => w.Type == "Sub").Select(s => s.Value).FirstOrDefault()!
        );

        var response = await messageRepository.GetUnseenMessagesCount(current_user);
        if (response.IsSuccess)
            await Clients.Caller.SendAsync("ShowUnseenCount", response.Data);
    }

    public async Task SendMessageToUser(string username, string message)
    {
        var current_user = Context.User!.Claims
            .Where(_ => _.Type == "Sub")
            .Select(_ => _.Value)
            .FirstOrDefault();
        var cur_user_obj = await shareSpaceDb.Users.FindAsync(Guid.Parse(current_user!));
        if (cur_user_obj is null)
        {
            return;
        }
        if (Context.UserIdentifier != username)
        {
            var response = await messageRepository.StoreMessage(
                new MessageDto
                {
                    From = Guid.Parse(current_user!),
                    To = username,
                    Text = message,
                }
            );

            if (response.IsSuccess)
            {
                await Clients
                    .User(username)
                    .SendAsync(
                        "ReceiveMessageFromUser",
                        Context.UserIdentifier,
                        message,
                        Guid.Parse(current_user!),
                        cur_user_obj.ProfilePicUrl
                    );
                await Clients.Caller.SendAsync(
                    "ReceiveMessageFromUser",
                    Context.UserIdentifier,
                    message,
                    Guid.Parse(current_user!),
                    cur_user_obj.ProfilePicUrl
                );
            }
        }
    }

    public async Task ShowUsersInChat(string username)
    {
        var current_user = Context.User!.Claims
            .Where(_ => _.Type == "Sub")
            .Select(_ => _.Value)
            .FirstOrDefault();

        var response = await messageRepository.GetUsersInChat(username);
        // foreach (var item in response.Data!)
        // {
        //     await Console.Out.WriteLineAsync(item.UserName!.ToString());
        //     await Console.Out.WriteLineAsync(item.Message);
        // }
        if (response.IsSuccess)
            await Clients.User(username).SendAsync("ReceiveUsersInFromUser", response.Data);
    }
}

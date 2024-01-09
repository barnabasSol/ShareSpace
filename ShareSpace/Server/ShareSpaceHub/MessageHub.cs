using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ShareSpace.Server.Repository.Contracts;
using ShareSpace.Shared.DTOs;

namespace ShareSpace.Server.ShareSpaceHub;

public class MessageHub : Hub
{
    private readonly IMessageRepository messageRepository;

    public MessageHub(IMessageRepository messageRepository)
    {
        this.messageRepository = messageRepository;
    }

    [Authorize]
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return base.OnDisconnectedAsync(exception);
    }

    [Authorize]
    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    [Authorize]
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

    [Authorize]
    public async Task GetUnseenMessagesCount()
    {
        Guid current_user = Guid.Parse(
            Context.User!.Claims.Where(w => w.Type == "Sub").Select(s => s.Value).FirstOrDefault()!
        );

        var response = await messageRepository.GetUnseenMessagesCount(current_user);
        if (response.IsSuccess)
            await Clients.Caller.SendAsync("ShowUnseenCount", response.Data);
    }

    [Authorize]
    public async Task SendMessageToUser(string username, string message)
    {
        var current_user = Context.User!.Claims
            .Where(_ => _.Type == "Sub")
            .Select(_ => _.Value)
            .FirstOrDefault();
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
                        Guid.Parse(current_user!)
                    );
                await Clients.Caller.SendAsync(
                    "ReceiveMessageFromUser",
                    Context.UserIdentifier,
                    message,
                    Guid.Parse(current_user!)
                );
            }
        }
    }

    [Authorize]
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

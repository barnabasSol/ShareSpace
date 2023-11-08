using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ShareSpace.Server.Repository.Contracts;
using ShareSpace.Shared.DTOs;

namespace ShareSpace.Server.ShareSpaceHub
{
    public class ShareSpaceHub : Hub
    {
        private readonly IMessageRepository messageRepository;

        public ShareSpaceHub(IMessageRepository messageRepository)
        {
            this.messageRepository = messageRepository;
        }

        [Authorize]
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", Context.UserIdentifier, message);
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
                    new MessageDto()
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
                        .SendAsync("ReceiveMessageFromUser", Context.UserIdentifier, message);
                    await Clients.Caller.SendAsync(
                        "ReceiveMessageFromUser",
                        Context.UserIdentifier,
                        message
                    );
                }
            }
        }

        [Authorize]
        public async Task NotifyUserMessage(string username, string message)
        {
            await Clients
                .User(username)
                .SendAsync("ReceiveNotificationFromUser", Context.UserIdentifier, message);
        }

        [Authorize]
        public async Task ShowUsersInChat(string username)
        {
            var current_user = Context.User!.Claims
                .Where(_ => _.Type == "Sub")
                .Select(_ => _.Value)
                .FirstOrDefault();

            var response = await messageRepository.GetUsersInChat(username);
            foreach (var item in response.Data!)
            {
                await Console.Out.WriteLineAsync(item.UserName!.ToString());
                await Console.Out.WriteLineAsync(item.Message);
            }
            if (response.IsSuccess)
            {
                await Clients.User(username).SendAsync("ReceiveUsersInFromUser", response.Data);
            }
        }
    }
}

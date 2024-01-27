using Microsoft.AspNetCore.SignalR.Client;
using ShareSpace.Shared.DTOs;

namespace ShareSpace.Client.Pages.UserPages;

public partial class ChatArea
{
    async Task StartChatSignalRConnectionAsync()
    {
        messageLabel = $"chat with @{username}";
        string token = await localStorage.GetItemAsync<string>("ShareSpaceAccessToken");
        hubConnection = new HubConnectionBuilder()
            .WithUrl(
                NavigationManager.ToAbsoluteUri("/messagehub"),
                opts =>
                {
                    opts.AccessTokenProvider = () => Task.FromResult(token)!;
                }
            )
            .WithAutomaticReconnect()
            .Build();

        var state = await authstate.GetAuthenticationStateAsync();
        string current_user_string = state.User.Claims
            .Where(_ => _.Type == "Sub")
            .Select(_ => _.Value)
            .FirstOrDefault()!;

        hubConnection.On<string, string, Guid, string>(
            "ReceiveMessageFromUser",
            (to_user, message, current_user, prof_pic) =>
            {
                if (username == to_user || current_user == Guid.Parse(current_user_string))
                {
                    messages.Add(
                        new MessageDto
                        {
                            Text = message!,
                            To = to_user!,
                            From = current_user,
                            SentDateTime = DateTime.Now,
                            ProfilePic = prof_pic
                        }
                    );
                    StateHasChanged();
                }
            }
        );

        hubConnection.On<List<MessageDto>>(
            "ShowMessages",
            (messages) =>
            {
                this.messages = messages;
                StateHasChanged();
            }
        );

        await hubConnection.StartAsync();
        await hubConnection.InvokeAsync("FetchMessagesOfUser", username);
    }
}

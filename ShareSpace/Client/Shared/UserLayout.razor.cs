using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using ShareSpace.Shared.DTOs;
using System.Security.Claims;

namespace ShareSpace.Client.Shared
{
    public partial class UserLayout
    {
        private HubConnection? hubConnection;
        private string? MessageCount;
        private int NotificationsCount;
        public bool IsConnected => hubConnection?.State == HubConnectionState.Connected;

        public ExtraUserInfoDto extraUserInfo = new();

        protected override async Task OnInitializedAsync()
        {
            var state = await authstate.GetAuthenticationStateAsync();
            string current_user_string = state.User.Claims
                .Where(_ => _.Type == "Sub")
                .Select(_ => _.Value)
                .FirstOrDefault()!;

            var response = await UserService.GetExtraUserInfo(Guid.Parse(current_user_string));
            if (response is not null)
            {
                if (response.IsSuccess)
                {
                    if (response.Data is not null)
                        extraUserInfo = response.Data;
                }
            }

            var notif_result = await NotificationService.GetNotificationsCount();
            if (notif_result.IsSuccess)
            {
                NotificationsCount = notif_result.Data;
            }
        }

        async void Logout()
        {
            await localStorage.RemoveItemAsync("ShareSpaceAccessToken");
            await localStorage.RemoveItemAsync("ShareSpaceRefreshToken");
            if (hubConnection is not null)
                await hubConnection.DisposeAsync();
            await authstate.GetAuthenticationStateAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
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

            hubConnection.On<string, string, Guid, string>(
                "ReceiveMessageFromUser",
                (user, message, current_user, prof_pic) =>
                {
                    string incoming_message = $"@{user}  {message}";
                    ShowSnackBarWithOptions(user, message, prof_pic, Variant.Filled);
                }
            );

            hubConnection.On<int>(
                "ShowUnseenCount",
                (count) =>
                {
                    MessageCount = count.ToString();
                    StateHasChanged();
                }
            );

            await hubConnection.StartAsync();

            if (firstRender)
            {
                await hubConnection.InvokeAsync("GetUnseenMessagesCount");
            }
        }

        private static UserInfo GetUserInfo(List<Claim> claims)
        {
            var Sub = claims.Where(_ => _.Type == "Sub").Select(_ => _.Value).FirstOrDefault();
            var Name = claims.Where(_ => _.Type == "Name").Select(_ => _.Value).FirstOrDefault();
            var UserName = claims
                .Where(_ => _.Type == "UserName")
                .Select(_ => _.Value)
                .FirstOrDefault();
            var Email = claims.Where(_ => _.Type == "Email").Select(_ => _.Value).FirstOrDefault();
            return new UserInfo()
            {
                Sub = Sub,
                Name = Name,
                UserName = UserName,
                Email = Email
            };
        }

        void ShowSnackBarWithOptions(
            string user,
            string message,
            string profile_pic,
            Variant variant
        )
        {
            SnackBar.Configuration.SnackbarVariant = variant;
            SnackBar.Configuration.VisibleStateDuration = 3000;
            SnackBar.Configuration.HideTransitionDuration = 500;
            SnackBar.Configuration.PositionClass = Defaults.Classes.Position.BottomRight;
            string display = $"""
<div>
    <div style='display: flex; align-items: center;'>
        <img src='{profile_pic}' alt='Profile Picture' style='width: 40px; height: 40px; border-radius: 50%; margin-right: 10px; object-fit: cover;'/>
        <h6>@{user}</h6>
    </div>
    <ul>
        <li>{message}</li>
    </ul>
</div>
""";

            SnackBar.Add(
                display,
                Severity.Normal,
                config =>
                {
                    config.HideIcon = true;
                    config.Onclick = snackbar =>
                    {
                        NavigationManager.NavigateTo($"/chat/{user}");
                        return Task.CompletedTask;
                    };
                }
            );
        }

        public async ValueTask DisposeAsync()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
        }
    }

    public class UserInfo
    {
        public string? Sub { get; set; }
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
    }
}

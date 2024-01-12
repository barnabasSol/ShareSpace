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
        private List<string> notifications = new();
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

            hubConnection.On<string, string, Guid>(
                "ReceiveMessageFromUser",
                (user, message, current_user) =>
                {
                    string incoming_message = $"@{user}  {message}";
                    ShowSnackBarWithOptions(incoming_message, Variant.Filled);
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

        void ShowSnackBarWithOptions(string message, Variant variant)
        {
            SnackBar.Configuration.SnackbarVariant = variant;
            SnackBar.Configuration.PositionClass = Defaults.Classes.Position.BottomRight;
            SnackBar.Configuration.VisibleStateDuration = 1000;
            SnackBar.Add($"{message}", Severity.Normal);
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

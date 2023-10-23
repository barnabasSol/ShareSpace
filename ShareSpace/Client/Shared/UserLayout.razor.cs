using MudBlazor;
using ShareSpace.Shared.DTOs;
using System.Security.Claims;

namespace ShareSpace.Client.Shared
{
    public partial class UserLayout
    {
        private readonly string[] messages = new[] { "hi", "hellow", "fuck you", "piece of shit" };

        public ExtraUserInfoDto extraUserInfo = new();

        protected override async Task OnInitializedAsync()
        {
            var response = await UserService.GetExtraUserInfo();
            if (response is not null)
            {
                if (response.IsSuccess)
                {
                    if (response.Data is not null)
                        extraUserInfo = response.Data;
                }
            }
            foreach (var message in messages)
            {
                ShowSnackBarWithOptions(message, Variant.Filled);
                await Task.Delay(2000);
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
    }

    public class UserInfo
    {
        public string? Sub { get; set; }
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
    }
}

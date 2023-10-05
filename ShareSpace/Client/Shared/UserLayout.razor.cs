using System.Security.Claims;

namespace ShareSpace.Client.Shared
{
    public partial class UserLayout
    {
        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }

        private static UserInfo GetUserInfo(List<Claim> claims)
        {
            var Sub = claims.Where(_ => _.Type == "Sub").Select(_ => _.Value).FirstOrDefault();
            var Name = claims.Where(_ => _.Type == "Name").Select(_ => _.Value).FirstOrDefault();
            var UserName = claims .Where(_ => _.Type == "UserName") .Select(_ => _.Value) .FirstOrDefault();
            var Email = claims.Where(_ => _.Type == "Email").Select(_ => _.Value).FirstOrDefault();
            return new UserInfo()
            {
                Sub = Sub,
                Name = Name,
                UserName = UserName,
                Email = Email
            };
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

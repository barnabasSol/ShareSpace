using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;

namespace ShareSpace.Client
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService localStorage;
        private readonly NavigationManager navigationManager;

        public CustomAuthenticationStateProvider(
            ILocalStorageService localStorage,
            NavigationManager navigationManager
        )
        {
            this.localStorage = localStorage;
            this.navigationManager = navigationManager;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string token = await localStorage.GetItemAsync<string>("ShareSpaceToken");
            if (string.IsNullOrEmpty(token))
            {
                navigationManager.NavigateTo("/");
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var claims = ParseClaimsFromJwt(token);
            var expClaim = claims.Where(_ => _.Type == "exp").Select(_ => _.Value).FirstOrDefault();

            if (expClaim != null)
            {
                var expDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim));
                if (expDate < DateTimeOffset.UtcNow)
                {
                    await localStorage.RemoveItemAsync("ShareSpaceToken");
                    navigationManager.NavigateTo("/sign-in");
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }
            }

            AuthenticationState state =
                new(new ClaimsPrincipal(new ClaimsIdentity(claims, "ShareSpaceTokenAuth")));

            NotifyAuthenticationStateChanged(Task.FromResult(state));
            //navigationManager.NavigateTo("/main");
            return await Task.FromResult(state);
        }

        private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
#pragma warning disable CS8604 // Possible null reference argument.
            return keyValuePairs!.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
#pragma warning restore CS8604 // Possible null reference argument.
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2:
                    base64 += "==";
                    break;
                case 3:
                    base64 += "=            ";
                    break;
            }
            return Convert.FromBase64String(base64);
        }

        public void NotifyAuthStateChange()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}

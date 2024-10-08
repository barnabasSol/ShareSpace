﻿using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;

namespace ShareSpace.Client;
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
        string token = await localStorage.GetItemAsync<string>("ShareSpaceAccessToken");
        if (string.IsNullOrEmpty(token))
        {
            navigationManager.NavigateTo("/");
            var logout_state = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            NotifyAuthenticationStateChanged(Task.FromResult(logout_state));
            return logout_state;
        }

        var claims = ParseClaimsFromJwt(token);
        var expClaim = claims.Where(_ => _.Type == "exp").Select(_ => _.Value).FirstOrDefault();

        if (expClaim is not null)
        {
            var expDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim));
            if (expDate < DateTimeOffset.UtcNow)
            {
                //await localStorage.RemoveItemAsync("ShareSpaceAccessToken");
                navigationManager.NavigateTo("/");
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        AuthenticationState state =
            new(new ClaimsPrincipal(new ClaimsIdentity(claims, "ShareSpaceTokenAuth")));

        NotifyAuthenticationStateChanged(Task.FromResult(state));
        return await Task.FromResult(state);
    }

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return keyValuePairs!.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!));
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
}

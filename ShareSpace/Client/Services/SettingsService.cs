using ShareSpace.Client.Services.Contracts;
using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;
using System.Net.Http.Json;

namespace ShareSpace.Client.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly IHttpClientFactory http_client;

        public SettingsService(IHttpClientFactory http)
        {
            http_client = http;
        }

        public async Task<ApiResponse<string>> UpdatePassword(UpdatePasswordDto passwordDto)
        {
            var http = http_client.CreateClient("ShareSpaceApi");
            var url = $"/Settings/password";
            var response = await http.PutAsJsonAsync(url, passwordDto);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
            return result!;
        }

        public async Task<ApiResponse<AuthResponse>> UpdateProfile(UpdateUserProfileDto profileDto)
        {
            var http = http_client.CreateClient("ShareSpaceApi");
            var url = $"/Settings/update-profile";
            var response = await http.PutAsJsonAsync(url, profileDto);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponse>>();
            return result!;
        }
    }
}

using ShareSpace.Client.Services.Contracts;
using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;
using System.Net.Http.Json;

namespace ShareSpace.Client.Services
{
    public class SuggestedUsersService : ISuggestedUsersService
    {
        private readonly IHttpClientFactory httpClient;

        public SuggestedUsersService(IHttpClientFactory httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<ApiResponse<IEnumerable<SuggestedUserDto>>> GetSuggestedUsers(Guid current_user)
        {
            var http = httpClient.CreateClient("ShareSpaceApi");
            var response = await http.GetAsync("/User/suggested-users");
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<SuggestedUserDto>>>();
            return result!;
        }
    }
}

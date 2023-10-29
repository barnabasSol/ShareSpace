using ShareSpace.Client.Services.Contracts;
using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;
using System.Net.Http.Json;

namespace ShareSpace.Client.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpClientFactory http_client;

        public UserService(IHttpClientFactory http)
        {
            http_client = http;
        }

        public async Task<ApiResponse<ExtraUserInfoDto>> GetExtraUserInfo()
        {
            var http = http_client.CreateClient("ShareSpaceApi");
            var response = await http.GetAsync("/User/extra-user-info");
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ExtraUserInfoDto>>();
            return result!;
        }

        public async Task<ApiResponse<IEnumerable<InterestsDto>>> GetInterests()
        {
            var http = http_client.CreateClient("ShareSpaceApi");
            var response = await http.GetAsync("/User/get-interests");
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<InterestsDto>>>();
            return result!;
        }

        public async Task<ApiResponse<string>> SendInterests(IEnumerable<InterestsDto> interests)
        {
            var http = http_client.CreateClient("ShareSpaceApi");
            var response = await http.PostAsJsonAsync("/User/store-interests", interests);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
            return result!;
        }
    }
}

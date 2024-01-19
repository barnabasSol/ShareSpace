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

        public async Task<ApiResponse<string>> FollowUser(Guid user_id)
        {
            var http = http_client.CreateClient("ShareSpaceApi");
            var url = $"/User/follow/{user_id}";
            var response = await http.PutAsJsonAsync(url, user_id);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
            return result!;
        }

        public async Task<ApiResponse<ExtraUserInfoDto>> GetExtraUserInfo(Guid user_id)
        {
            var http = http_client.CreateClient("ShareSpaceApi");
            var url = $"/User/extra-info/{user_id}";
            var response = await http.GetAsync(url);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ExtraUserInfoDto>>();
            return result!;
        }

        public async Task<ApiResponse<List<FollowerUserDto>>> GetFollowers()
        {
            var http = http_client.CreateClient("ShareSpaceApi");
            var response = await http.GetAsync("/User/followers");
            var result = await response.Content.ReadFromJsonAsync<
                ApiResponse<List<FollowerUserDto>>
            >();
            return result!;
        }

        public async Task<ApiResponse<List<FollowerUserDto>>> GetFollowing()
        {
            var http = http_client.CreateClient("ShareSpaceApi");
            var response = await http.GetAsync("/User/following");
            var result = await response.Content.ReadFromJsonAsync<
                ApiResponse<List<FollowerUserDto>>
            >();
            return result!;
        }

        public async Task<ApiResponse<IEnumerable<InterestsDto>>> GetInterests()
        {
            var http = http_client.CreateClient("ShareSpaceApi");
            var response = await http.GetAsync("/User/interests");
            var result = await response.Content.ReadFromJsonAsync<
                ApiResponse<IEnumerable<InterestsDto>>
            >();
            return result!;
        }

        public async Task<ApiResponse<List<SuggestedUserDto>>> GetSuggestedUsers()
        {
            var http = http_client.CreateClient("ShareSpaceApi");
            var response = await http.GetAsync("/User/suggested-users");
            var result = await response.Content.ReadFromJsonAsync<
                ApiResponse<List<SuggestedUserDto>>
            >();
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

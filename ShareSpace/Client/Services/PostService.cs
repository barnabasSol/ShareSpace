using ShareSpace.Client.Services.Contracts;
using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;
using System.Net.Http.Json;

namespace ShareSpace.Client.Services
{
    public class PostService : IPostService
    {
        private readonly IHttpClientFactory http_client;

        public PostService(IHttpClientFactory http)
        {
            http_client = http;
        }

        public async Task<ApiResponse<string>> CreatePost(CreatePostDto post)
        {
            var http = http_client.CreateClient("ShareSpaceApi");
            var response = await http.PostAsJsonAsync("/Post/create-post", post);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
            return result!;
        }


    }
}

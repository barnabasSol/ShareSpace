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
            var response = await http.PostAsJsonAsync("/Post/create", post);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
            return result!;
        }

        public async Task<ApiResponse<string>> DeletePost(Guid post_id)
        {
            var http = http_client.CreateClient("ShareSpaceApi");
            var url = $"/Post/delete/{post_id}";
            var response = await http.DeleteAsync(url);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
            return result!;
        }

        public async Task<ApiResponse<List<PostDto>>> GetPosts()
        {
            var http = http_client.CreateClient("ShareSpaceApi");
            var response = await http.GetAsync("/Post/posts");
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<PostDto>>>();
            return result!;
        }

        public async Task<ApiResponse<string>> UpdateLike(LikedPostDto likedPost)
        {
            var http = http_client.CreateClient("ShareSpaceApi");
            var url = "/Post/like";
            var response = await http.PutAsJsonAsync(url, likedPost);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
            return result!;
        }
    }
}

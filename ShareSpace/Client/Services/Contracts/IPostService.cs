using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Client.Services.Contracts
{
    public interface IPostService
    {
        Task<ApiResponse<string>> CreatePost(CreatePostDto post);
        Task<ApiResponse<List<PostDto>>> GetPosts();
    }
}

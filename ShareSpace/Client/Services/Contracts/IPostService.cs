using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Client.Services.Contracts;

public interface IPostService
{
    Task<ApiResponse<string>> CreatePost(CreatePostDto post);
    Task<ApiResponse<List<PostDto>>> GetPosts();
    Task<ApiResponse<List<PostDto>>> GetTrendingPosts();
    Task<ApiResponse<PostDetailDto>> GetPostDetails(Guid post_id);
    Task<ApiResponse<string>> DeletePost(Guid post_id);
    Task<ApiResponse<string>> UpdateLike(LikedPostDto likedPost);
}

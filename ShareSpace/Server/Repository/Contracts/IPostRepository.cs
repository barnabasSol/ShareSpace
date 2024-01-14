using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Server.Repository.Contracts;

public interface IPostRepository
{
    Task<ApiResponse<string>> CreatePost(CreatePostDto post);
    Task<ApiResponse<IEnumerable<PostDto>>> GetPosts(Guid current_user);
    Task<ApiResponse<PostDetailDto>> GetPost(Guid post_id);
    Task<ApiResponse<string>> DeletePost(Guid post_id);
    Task<ApiResponse<string>> UpdateLike(Guid post_id, Guid user_id);
}

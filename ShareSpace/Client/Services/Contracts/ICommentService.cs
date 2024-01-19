using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Client.Services.Contracts;

public interface ICommentService
{
    Task<ApiResponse<Guid>> AddComment(CommentAddDto comment);
    Task<ApiResponse<string>> DeleteComment(Guid post_id);
}

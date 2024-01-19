using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Server.Repository.Contracts;

public interface ICommentRepository
{
    Task<ApiResponse<string>> DeleteComment(Guid comment_id);
    Task<ApiResponse<Guid>> AddComment(CommentAddDto comment, Guid user_id);
}

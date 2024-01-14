using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Server.Repository.Contracts;

public interface ICommentRepository
{
    Task<ApiResponse<string>> DeleteComment(Guid comment_id);
    Task<ApiResponse<string>> AddComment(CommentAddDto comment);
}

using ShareSpace.Server.Data;
using ShareSpace.Server.Repository.Contracts;
using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Server.Repository;

public class CommentRepository : ICommentRepository
{
    private readonly ShareSpaceDbContext shareSpaceDb;

    public CommentRepository(ShareSpaceDbContext shareSpaceDb)
    {
        this.shareSpaceDb = shareSpaceDb;
    }

    public Task<ApiResponse<string>> AddComment(CommentAddDto comment)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<string>> DeleteComment(Guid comment_id)
    {
        throw new NotImplementedException();
    }
}

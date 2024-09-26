using Microsoft.EntityFrameworkCore;
using ShareSpace.Server.Data;
using ShareSpace.Server.Entities;
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

    public async Task<ApiResponse<Guid>> AddComment(CommentAddDto comment, Guid user_id)
    {
        try
        {
            var new_comment = new Comment
            {
                Content = comment.Content,
                PostId = comment.PostId,
                UserId = user_id
            };
            await shareSpaceDb.Comments.AddAsync(new_comment);
            await shareSpaceDb.SaveChangesAsync();

            return new ApiResponse<Guid> { IsSuccess = true, Data = new_comment.Id };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<ApiResponse<string>> DeleteComment(Guid comment_id)
    {
        try
        {
            await shareSpaceDb.Comments.Where(w => w.Id == comment_id).ExecuteDeleteAsync();
            return new ApiResponse<string> { IsSuccess = true };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}

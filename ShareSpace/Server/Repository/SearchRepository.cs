using Microsoft.EntityFrameworkCore;
using ShareSpace.Server.Data;
using ShareSpace.Server.Repository.Contracts;
using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Server.Repository;

public class SearchRepository : ISearchRepository
{
    private readonly ShareSpaceDbContext shareSpaceDb;

    public SearchRepository(ShareSpaceDbContext shareSpaceDb)
    {
        this.shareSpaceDb = shareSpaceDb;
    }

    public async Task<ApiResponse<IEnumerable<PostSearchDto>>> SearchPost(
        string query,
        Guid current_user
    )
    {
        try
        {
            var posts = await shareSpaceDb.Posts
                .Where(w => w.Content!.ToLower().Contains(query))
                .Include(i => i.User)
                .Include(i => i.PostImages)
                .Include(i => i.Comments)
                .ToListAsync();

            if (!posts.Any())
            {
                return new ApiResponse<IEnumerable<PostSearchDto>>
                {
                    IsSuccess = true,
                    Message = "No Result Found",
                    Data = Enumerable.Empty<PostSearchDto>()
                };
            }
            return new ApiResponse<IEnumerable<PostSearchDto>>
            {
                IsSuccess = true,
                Data = posts.Select(
                    s =>
                        new PostSearchDto
                        {
                            TextContent = s.Content,
                            PostUserProfilePicUrl = s.User?.ProfilePicUrl,
                            PostedName = s.User!.Name,
                            PostedUsername = s.User!.UserName,
                            PostedUserId = s.UserId,
                            PostId = s.Id,
                            PostPictureUrls = s.PostImages?.Select(i => i.ImageUrl),
                            LikesCount = s.Likes,
                            ViewsCount = s.Views,
                            CommentsCount = s.Comments?.Count ?? 0,
                            PostedDateTime = s.CreatedAt,
                            IsLikedByCurrentUser = shareSpaceDb.LikedPosts.Any(
                                a => a.PostId == s.Id && a.UserId == current_user
                            )
                        }
                )
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<ApiResponse<IEnumerable<UserSearchDto>>> SearchUser(
        string query_string,
        Guid current_user
    )
    {
        try
        {
            var users = await shareSpaceDb.Users
                .Where(
                    w =>
                        w.UserName.ToLower().Contains(query_string)
                        || w.Name.ToLower().Contains(query_string)
                        || w.Bio!.ToLower().Contains(query_string)
                )
                .ToListAsync();
            if (!users.Any())
            {
                return new ApiResponse<IEnumerable<UserSearchDto>>
                {
                    IsSuccess = true,
                    Message = "User Doesn't Exist",
                    Data = Enumerable.Empty<UserSearchDto>()
                };
            }
            return new ApiResponse<IEnumerable<UserSearchDto>>
            {
                IsSuccess = true,
                Data = users.Select(
                    s =>
                        new UserSearchDto
                        {
                            UserId = s.UserId,
                            ProfilePic = s.ProfilePicUrl,
                            UserName = s.UserName,
                            Name = s.Name,
                            IsBeingFollowed = shareSpaceDb.Followers.Any(
                                a => a.FollowedId == s.UserId && a.FollowerId == current_user
                            )
                        }
                )
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}

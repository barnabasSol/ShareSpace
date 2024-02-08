using Microsoft.EntityFrameworkCore;
using ShareSpace.Server.Data;
using ShareSpace.Server.Entities;
using ShareSpace.Server.Extensions;
using ShareSpace.Server.Repository.Contracts;
using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Server.Repository;

public class PostRepository : IPostRepository
{
    private readonly ShareSpaceDbContext shareSpaceDb;
    private readonly IWebHostEnvironment webHost;

    public PostRepository(ShareSpaceDbContext shareSpaceDb, IWebHostEnvironment webHost)
    {
        this.shareSpaceDb = shareSpaceDb;
        this.webHost = webHost;
    }

    public async Task<ApiResponse<string>> CreatePost(CreatePostDto post)
    {
        using var transaction = await shareSpaceDb.Database.BeginTransactionAsync();
        try
        {
            if (post.PostFiles is not null)
            {
                Post NewPost = new() { Content = post.TextContent, UserId = post.PostedUserId };

                shareSpaceDb.Posts.Add(NewPost);
                await shareSpaceDb.SaveChangesAsync();
                foreach (var file in post.PostFiles)
                {
                    string FileExtension = file.Type.GetFileExtension();
                    string image_url = $"Uploads/PostPictures/{Guid.NewGuid()}.{FileExtension}";
                    string webRootPath = webHost.WebRootPath;
                    string FileName = Path.Combine(webRootPath, image_url);

                    var newPostImage = new PostImage { ImageUrl = image_url, Post = NewPost };

                    shareSpaceDb.PostImages.Add(newPostImage);

                    using var FileStream = System.IO.File.Create(FileName);
                    await FileStream.WriteAsync(file.ImageBytes);
                }
                if (post.ExtractTags().Any())
                {
                    foreach (var tag in post.ExtractTags())
                    {
                        await shareSpaceDb.PostTags.AddAsync(
                            new PostTag { TagName = tag, PostId = NewPost.Id }
                        );
                    }
                }

                await shareSpaceDb.SaveChangesAsync();
                await transaction.CommitAsync();
            }

            return new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "",
                Data = ""
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception(ex.Message);
        }
    }

    public async Task<ApiResponse<string>> DeletePost(Guid post_id)
    {
        using var transaction = await shareSpaceDb.Database.BeginTransactionAsync();
        try
        {
            var post = await shareSpaceDb.Posts.FindAsync(post_id);
            string webRootPath = webHost.WebRootPath;
            if (post is not null)
            {
                var postImages = await shareSpaceDb.PostImages
                    .Where(pi => pi.PostId == post_id)
                    .ToListAsync();

                //deletes files from the damn directory
                foreach (var postImage in postImages)
                {
                    var imagePath = Path.Combine(webRootPath, postImage.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                        System.IO.File.Delete(imagePath);
                }
                shareSpaceDb.Posts.Remove(post);

                await shareSpaceDb.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ApiResponse<string> { IsSuccess = true, Message = "", };
            }
            return new ApiResponse<string> { IsSuccess = false, Message = "item doesn't exist", };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception(ex.Message);
        }
    }

    public async Task<ApiResponse<PostDetailDto>> GetPost(Guid post_id, Guid current_user)
    {
        try
        {
            var post = await shareSpaceDb.Posts
                .Include(i => i.User)
                .Include(i => i.Comments!)
                .ThenInclude(c => c.User)
                .Include(i => i.PostImages)
                .FirstOrDefaultAsync(f => f.Id == post_id);

            if (post is null)
            {
                return new ApiResponse<PostDetailDto>
                {
                    IsSuccess = false,
                    Message = "post doesn't exist",
                };
            }

            var commentDtos =
                post.Comments
                    ?.Select(
                        s =>
                            new CommentDto
                            {
                                CommentId = s.Id,
                                UserId = s.User!.UserId,
                                UserName = s.User?.UserName!,
                                Name = s.User?.Name!,
                                Content = s.Content,
                                UserProfilePicUrl = s.User?.ProfilePicUrl,
                                CommentedAt = s.CreatedAt
                            }
                    )
                    .ToList() ?? new List<CommentDto>();

            return new ApiResponse<PostDetailDto>
            {
                IsSuccess = true,
                Data = new PostDetailDto
                {
                    TextContent = post.Content,
                    PostUserProfilePicUrl = post.User?.ProfilePicUrl,
                    PostedName = post.User?.Name!,
                    PostedUsername = post.User?.UserName!,
                    PostedUserId = post.UserId,
                    PostId = post.Id,
                    PostPictureUrls =
                        post.PostImages?.Select(i => i.ImageUrl) ?? Enumerable.Empty<string>(),
                    LikesCount = post.Likes,
                    ViewsCount = post.Views,
                    CommentsCount = post.Comments?.Count ?? 0,
                    PostedDateTime = post.CreatedAt,
                    IsLikedByCurrentUser = shareSpaceDb.LikedPosts.Any(
                        a => a.PostId == post.Id && a.UserId == current_user
                    ),
                    Comments = commentDtos
                }
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<ApiResponse<IEnumerable<PostDto>>> GetPosts(Guid current_user)
    {
        try
        {
            var followings = await shareSpaceDb.Followers
                .Where(w => w.FollowerId == current_user)
                .Select(s => s.FollowedId)
                .ToListAsync();

            var currentUserInterests = await shareSpaceDb.UserInterests
                .Where(w => w.UserId == current_user)
                .Select(s => s.InterestId)
                .ToListAsync();

            var usersWithSameInterests = await shareSpaceDb.UserInterests
                .Where(w => currentUserInterests.Contains(w.InterestId))
                .Select(s => s.UserId)
                .Distinct()
                .ToListAsync();

            var likedTags = await shareSpaceDb.LikedPosts
                .Where(lp => lp.UserId == current_user)
                .SelectMany(lp => lp.Post!.PostTags!)
                .Select(pt => pt.TagName)
                .ToListAsync();

            var posts = await shareSpaceDb.Posts
                .Include(i => i.User)
                .Include(i => i.PostImages)
                .Include(i => i.Comments)
                .Include(i => i.PostTags)
                .Where(
                    w =>
                        followings.Contains(w.UserId)
                        || w.UserId == current_user
                        || usersWithSameInterests.Contains(w.UserId)
                        || w.PostTags!.Any(pt => likedTags.Contains(pt.TagName))
                )
                .ToListAsync();

            return new ApiResponse<IEnumerable<PostDto>>
            {
                IsSuccess = true,
                Message = "",
                Data = posts.ToPostDto(shareSpaceDb.LikedPosts, current_user)
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<ApiResponse<IEnumerable<PostDto>>> GetTrendingPosts(Guid current_user)
    {
        try
        {
            var currentDate = DateTime.Now;
            var posts = await shareSpaceDb.Posts
                .Include(i => i.User)
                .Include(i => i.PostImages)
                .Include(i => i.Comments)
                .Include(i => i.LikedPosts)
                .Where(p => (currentDate - p.CreatedAt).TotalDays <= 7)
                .OrderByDescending(p => p.LikedPosts!.Count)
                .ThenByDescending(p => p.CreatedAt)
                .ThenByDescending(p => p.Comments!.Count)
                .Take(3)
                .ToListAsync();

            return new ApiResponse<IEnumerable<PostDto>>
            {
                IsSuccess = true,
                Message = "",
                Data = posts
                    .Select(
                        s =>
                            new PostDto
                            {
                                TextContent = s.Content,
                                PostUserProfilePicUrl = s.User?.ProfilePicUrl,
                                PostedName = s.User!.Name,
                                PostedUsername = s.User!.UserName,
                                PostedUserId = s.UserId,
                                PostId = s.Id,
                                PostPictureUrls =
                                    s.PostImages?.Select(i => i.ImageUrl)
                                    ?? Enumerable.Empty<string>(),
                                LikesCount = s.Likes,
                                ViewsCount = s.Views,
                                CommentsCount = s.Comments?.Count ?? 0,
                                PostedDateTime = s.CreatedAt,
                                IsLikedByCurrentUser = shareSpaceDb.LikedPosts.Any(
                                    a => a.PostId == s.Id && a.UserId == current_user
                                ),
                                LikedTimeStamp = s.LikedPosts!
                                    .Select(s => s.CreatedAt)
                                    .FirstOrDefault()
                            }
                    )
                    .OrderByDescending(o => o.LikesCount)
                    .ThenByDescending(o => o.PostedDateTime)
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<ApiResponse<string>> UpdateLike(Guid post_id, Guid user_id)
    {
        using var transaction = await shareSpaceDb.Database.BeginTransactionAsync();
        try
        {
            var liked_post = await shareSpaceDb.LikedPosts.FirstOrDefaultAsync(
                f => f.UserId == user_id && f.PostId == post_id
            );

            if (liked_post is not null)
            {
                shareSpaceDb.LikedPosts.Remove(liked_post);
            }
            else
            {
                await shareSpaceDb.LikedPosts.AddAsync(
                    new LikedPost { UserId = user_id, PostId = post_id, }
                );
            }

            await shareSpaceDb.SaveChangesAsync();

            var postLikeCount = await shareSpaceDb.LikedPosts.CountAsync(
                lp => lp.PostId == post_id
            );
            await shareSpaceDb.Posts
                .Where(p => p.Id == post_id)
                .ExecuteUpdateAsync(s => s.SetProperty(p => p.Likes, postLikeCount));

            await transaction.CommitAsync();
            return new ApiResponse<string> { IsSuccess = true };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new ApiResponse<string> { IsSuccess = false, Message = ex.Message };
        }
    }
}

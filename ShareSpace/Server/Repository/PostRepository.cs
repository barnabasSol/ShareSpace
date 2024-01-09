using Microsoft.EntityFrameworkCore;
using ShareSpace.Server.Data;
using ShareSpace.Server.Entities;
using ShareSpace.Server.Repository.Contracts;
using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Server.Repository
{
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

                    foreach (var file in post.PostFiles)
                    {
                        string FileExtension = file.Type.ToLower() switch
                        {
                            string type when type.Contains("png") => "png",
                            string type when type.Contains("jpeg") => "jpeg",
                            string type when type.Contains("webp") => "webp",
                            _ => throw new Exception("Invalid file format!")
                        };
                        string image_url = $"Uploads/PostPictures/{Guid.NewGuid()}.{FileExtension}";
                        string webRootPath = webHost.WebRootPath;
                        string FileName = Path.Combine(webRootPath, image_url);

                        var newPostImage = new PostImage
                        {
                            ImageUrl = image_url,
                            Post = NewPost 
                        };

                        shareSpaceDb.PostImages.Add(newPostImage);

                        using var FileStream = System.IO.File.Create(FileName);
                        await FileStream.WriteAsync(file.ImageBytes);
                    }

                    await shareSpaceDb.SaveChangesAsync();
                    await transaction.CommitAsync();
                }

                return new ApiResponse<string>()
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
            try
            {
                var post = await shareSpaceDb.Posts.FindAsync(post_id);
                if (post is not null)
                {
                    shareSpaceDb.Posts.Remove(post);
                    await shareSpaceDb.SaveChangesAsync();
                    return new ApiResponse<string>() { IsSuccess = true, Message = "", };
                }
                return new ApiResponse<string>()
                {
                    IsSuccess = false,
                    Message = "item doesn't exist",
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
                var posts = await shareSpaceDb.Posts
                    .Include(i => i.User)
                    .Include(i => i.PostImages)
                    .ToListAsync();

                return new ApiResponse<IEnumerable<PostDto>>
                {
                    IsSuccess = true,
                    Message = "",
                    Data = posts.Select(
                        s =>
                            new PostDto
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
                                PostedDateTime = s.CreatedAt
                            }
                    ).OrderByDescending(o => o.PostedDateTime)
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

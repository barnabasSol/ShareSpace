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
            using var transaction = shareSpaceDb.Database.BeginTransaction();
            try
            {
                if (post.PostFiles is not null)
                {
                    Guid post_id = Guid.NewGuid();
                    await shareSpaceDb.Posts.AddAsync(
                        new Post()
                        {
                            Id = post_id,
                            Content = post.TextContent,
                            UserId = post.PostedUserId,
                        }
                    );
                    foreach (var file in post.PostFiles)
                    {
                        Guid guid = Guid.NewGuid();
                        string FileExtension = file.Type.ToLower() switch
                        {
                            string type when type.Contains("png") => "png",
                            string type when type.Contains("jpg") => "jpg",
                            string type when type.Contains("webp") => "webp",
                            _ => throw new Exception("Invalid file format!")
                        };

                        string webRootPath = webHost.WebRootPath;
                        string FileName = Path.Combine(
                            webRootPath,
                            $"Uploads/PostPictures/{guid}.{FileExtension}"
                        );

                        await shareSpaceDb.PostImages.AddAsync(
                            new PostImage()
                            {
                                PostId = post_id,
                                ImageUrl = $"Uplaods/PostPictures/{guid}.{FileExtension}"
                            }
                        );
                        using var FileStream = System.IO.File.Create(FileName);
                        await FileStream.WriteAsync(file.ImageBytes);
                    }
                    await shareSpaceDb.SaveChangesAsync();
                    transaction.Commit();
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
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        public Task<ApiResponse<PostDto>> GetPosts()
        {
            throw new NotImplementedException();
        }
    }
}

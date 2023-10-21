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

        public PostRepository(ShareSpaceDbContext shareSpaceDb)
        {
            this.shareSpaceDb = shareSpaceDb;
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
                        string FileExtension = file.Type.ToLower().Contains("png") ? "png" : "jpg";
                        string FileName =
                            $"C:\\Users\\Barnabas Solomon\\OneDrive\\Desktop\\web\\ASPNET projects\\ShareSpaceSolution\\ShareSpace\\Server\\Data\\Uploads\\PostPictures\\{guid}.{FileExtension}";

                        await shareSpaceDb.PostImages.AddAsync(
                            new PostImage()
                            {
                                PostId = post_id,
                                ImageUrl = FileName[FileName.IndexOf("\\Server")..]
                            }
                        );
                        using var FileStream = File.Create(FileName);
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

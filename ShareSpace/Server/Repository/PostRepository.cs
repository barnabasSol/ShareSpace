using ShareSpace.Server.Data;
using ShareSpace.Server.Repository.Contracts;

namespace ShareSpace.Server.Repository{
    public class PostRepository : IPostRepository
    {
        private readonly ShareSpaceDbContext shareSpaceDb;

        public PostRepository(ShareSpaceDbContext shareSpaceDb)
        {
            this.shareSpaceDb = shareSpaceDb;
        }
    }
}
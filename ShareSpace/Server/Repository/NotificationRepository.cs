using ShareSpace.Server.Data;
using ShareSpace.Server.Repository.Contracts;

namespace ShareSpace.Server.Repository
{
    public class NotificationRepository : INotificationRepostiory
    {
        private readonly ShareSpaceDbContext shareSpaceDb;

        public NotificationRepository(ShareSpaceDbContext shareSpaceDb)
        {
            this.shareSpaceDb = shareSpaceDb;
        }
    }
}

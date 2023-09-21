using Microsoft.EntityFrameworkCore;

namespace ShareSpace.Server.Data
{
    public class ShareSpaceDbContext : DbContext
    {
        public ShareSpaceDbContext (DbContextOptions<ShareSpaceDbContext> options) : base(options) { }
    }
}

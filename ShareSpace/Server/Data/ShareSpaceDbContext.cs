using Microsoft.EntityFrameworkCore;
using ShareSpace.Server.Entities;

namespace ShareSpace.Server.Data
{
    public class ShareSpaceDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Comment> Comments { get; set; }   
        public DbSet<Follower> Followers { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<LikedPost> LikedPosts { get; set; }
        public ShareSpaceDbContext (DbContextOptions<ShareSpaceDbContext> options) : base(options) { }
    }
}

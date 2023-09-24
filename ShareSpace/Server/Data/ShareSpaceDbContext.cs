using Microsoft.EntityFrameworkCore;
using ShareSpace.Server.Entities;

namespace ShareSpace.Server.Data
{
    public class ShareSpaceDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<LikedPost> LikedPosts { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Comment> Comments { get; set; }

        //public DbSet<Follower> Followers { get; set; }
        //public DbSet<Notification> Notifications { get; set; }
        //public DbSet<NotificationType> NotificationTypes { get; set; }
        //public DbSet<Tag> Tags { get; set; }
        //public DbSet<PostTag> PostTags { get; set; }

        public ShareSpaceDbContext(DbContextOptions<ShareSpaceDbContext> options)
            : base(options) { }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LikedPost>().HasKey(lp => new { lp.UserId, lp.PostId });

            modelBuilder.Entity<User>(u =>
            {
                u.Property(e => e.CreatedAt).HasDefaultValueSql("Now()");
                u.Property(e => e.UserId).HasDefaultValueSql("uuid_generate_v4()");
            });

            modelBuilder.Entity<Post>(p =>
            {
                p.Property(c => c.CreatedAt).HasDefaultValueSql("Now()");
                p.Property(i => i.Id).HasDefaultValueSql("uuid_generate_v4()");
                p.Property(l => l.Likes).HasDefaultValue("0");
            });

            modelBuilder.Entity<Comment>(c =>
            {
                c.Property(c => c.CreatedAt).HasDefaultValueSql("Now()");
                c.Property(i => i.Id).HasDefaultValueSql("uuid_generate_v4()");
                c.Property(l => l.Likes).HasDefaultValue("0");
            });

            modelBuilder.Entity<Message>(m =>
            {
                m.Property(c => c.CreatedAt).HasDefaultValueSql("Now()");
                m.Property(i => i.MessageId).HasDefaultValueSql("uuid_generate_v4()");
                m.Property(l => l.Seen).HasDefaultValue("FALSE");
                m.HasOne("ShareSpace.Server.Entities.User", "Receiver")
                    .WithMany("Messages")
                    .HasForeignKey("ReceiverId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });


        }
    }
}

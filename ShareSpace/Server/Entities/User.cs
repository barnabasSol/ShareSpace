using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShareSpace.Server.Entities
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("username", TypeName = "VARCHAR(100)")]
        public required string UserName { get; set; }

        [Column("name", TypeName = "VARCHAR(100)")]
        public required string Name { get; set; }

        [Column("email", TypeName = "VARCHAR(150)")]
        public required string Email { get; set; }

        [Column("password_hash", TypeName = "TEXT")]
        public required string PasswordHash { get; set; }

        [Column("bio")]
        public string? Bio { get; set; }

        [Column("profile_pic_url")]
        public string? ProfilePicUrl { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<LikedPost>? LikedPosts { get; set; }
        public virtual ICollection<Post>? Posts { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<Message>? Messages { get; set; }
        public virtual ICollection<Follower>? Followers { get; set; }
        public virtual ICollection<Notification>? Notifications { get; set; }
        public virtual ICollection<RefreshToken>? RefreshTokens { get; set; }
        public virtual ICollection<ViewedPost>? ViewedPosts { get; set; }
        public virtual ICollection<UserInterest>? UserInterests { get; set; }
        public virtual ICollection<UserRole>? Roles { get; set; }
    }
}

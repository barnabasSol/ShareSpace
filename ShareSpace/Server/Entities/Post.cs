using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShareSpace.Server.Entities
{
    [Table("posts")]
    public class Post
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("content")]
        public string? Content { get; set; }

        [Column("likes")]
        public int Likes { get; set; }

        [Column("views")]
        public int Views { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("user_id")]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        public User? User { get; set; }

        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<ViewedPost>? ViewedPosts { get; set; }
        public virtual ICollection<PostImage>? PostImages { get; set; }
        public virtual ICollection<PostTag>? PostTags { get; set; }
        public virtual ICollection<LikedPost>? LikedPosts { get; set; }
    }
}

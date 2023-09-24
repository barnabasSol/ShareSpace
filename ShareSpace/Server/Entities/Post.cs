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

        [Column("image_url")]
        public string? ImageUrl { get; set; }

        [Column("likes")]
        public int Likes { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("user_id")]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        public User? User { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShareSpace.Server.Entities
{
    [Table("comments")]
    public class Comment
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("content")]
        public required string Content { get; set; }

        [Column("likes")]
        public int Likes { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("user_id")]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [Column("post_id")]
        [ForeignKey("Post")]
        public Guid PostId { get; set; }

        public User? User { get; set; }
        public Post? Post { get; set; }
    }
}

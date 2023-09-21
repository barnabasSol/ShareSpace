using System.ComponentModel.DataAnnotations.Schema;

namespace ShareSpace.Server.Entities
{
    public class Comment
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("post_id")]
        public Guid PostId { get; set; }
        [Column("user_id")]
        public Guid UserId { get; set; }
        [Column("content")]
        public required string Content { get; set; }
        [Column("likes")]
        public int Likes { get; set; } = 0;
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}

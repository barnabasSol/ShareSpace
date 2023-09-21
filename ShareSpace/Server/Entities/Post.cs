using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShareSpace.Server.Entities
{
    public class Post
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("content")]
        public string? Content { get; set; }

        [Column("content")]
        public string? ImageUrl { get; set; }

        [Column("likes")]
        public int Likes { get; set; } = 0;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}

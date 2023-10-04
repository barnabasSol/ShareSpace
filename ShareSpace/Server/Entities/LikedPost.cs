using System.ComponentModel.DataAnnotations.Schema;

namespace ShareSpace.Server.Entities
{
    [Table("liked_posts")]
    public class LikedPost
    {
        [Column("user_id")]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [Column("post_id")]
        [ForeignKey("Post")]
        public Guid PostId { get; set; }
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        public User? User { get; set; }
        public Post? Post { get; set; }

    }
}

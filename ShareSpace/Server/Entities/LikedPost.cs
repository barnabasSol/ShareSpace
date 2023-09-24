using System.ComponentModel.DataAnnotations.Schema;

namespace ShareSpace.Server.Entities
{
    [Table("liked_posts")]
    public class LikedPost
    {
        [Column("user_id")]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [ForeignKey("Post")]
        [Column("post_id")]
        public Guid PostId { get; set; }

        public User? User { get; set; }
        public Post? Post { get; set; }

    }
}

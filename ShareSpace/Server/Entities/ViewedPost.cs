using System.ComponentModel.DataAnnotations.Schema;

namespace ShareSpace.Server.Entities
{
    [Table("viewed_posts")]
    public class ViewedPost
    {
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

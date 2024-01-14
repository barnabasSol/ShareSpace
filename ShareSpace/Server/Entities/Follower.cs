using System.ComponentModel.DataAnnotations.Schema;

namespace ShareSpace.Server.Entities
{
    [Table("followers")]
    public class Follower
    {
        [Column("followed_id")]
        [ForeignKey("FollowedUser")]
        public Guid FollowedId { get; set; }

        [Column("follower_id")]
        [ForeignKey("FollowerUser")]
        public Guid FollowerId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        public User? FollowedUser { get; set; }
        public User? FollowerUser { get; set; }
    }
}

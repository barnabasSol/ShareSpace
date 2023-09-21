using System.ComponentModel.DataAnnotations.Schema;

namespace ShareSpace.Server.Entities
{
    public class Follower
    {
        [Column("followed_id")]
        public Guid FollowedId { get; set; }
        [Column("follower_id")]
        public Guid FollowerId { get; set; }


    }
}

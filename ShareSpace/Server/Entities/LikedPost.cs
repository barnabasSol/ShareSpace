using System.ComponentModel.DataAnnotations.Schema;

namespace ShareSpace.Server.Entities
{
    public class LikedPost
    {
        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("post_id")]
        public Guid PostId { get; set; }
    }
}

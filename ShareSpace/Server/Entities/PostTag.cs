using System.ComponentModel.DataAnnotations.Schema;

namespace ShareSpace.Server.Entities
{
    public class PostTag
    {
        [Column("post_id")]
        public Guid PostId { get; set; }

        [Column("tag_id")]
        public Guid TagId { get; set; }
    }
}

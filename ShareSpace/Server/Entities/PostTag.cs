using System.ComponentModel.DataAnnotations.Schema;

namespace ShareSpace.Server.Entities
{
    [Table("post_tags")]
    public class PostTag
    {
        [Column("tag_id")]
        public Guid TagId { get; set; }

        [Column("post_id")]
        [ForeignKey("Post")]
        public Guid PostId { get; set; }

        public Post? Post { get; set; }
    }
}

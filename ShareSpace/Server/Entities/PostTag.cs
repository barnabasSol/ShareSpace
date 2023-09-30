using System.ComponentModel.DataAnnotations.Schema;

namespace ShareSpace.Server.Entities
{
    [Table("post_tags")]
    public class PostTag
    {
        [Column("tag_id")]
        [ForeignKey("Tag")]
        public Guid TagId { get; set; }

        [Column("post_id")]
        [ForeignKey("Post")]
        public Guid PostId { get; set; }

        public Tag? Tag { get; set; }
        public Post? Post { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShareSpace.Server.Entities;

[Table("post_tags")]
public class PostTag
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("tag_name")]
    public required string TagName { get; set; }

    [Column("post_id")]
    [ForeignKey("Post")]
    public Guid PostId { get; set; }

    public Post? Post { get; set; }
}

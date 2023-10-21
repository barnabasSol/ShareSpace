using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShareSpace.Server.Entities
{
    [Table("post_images")]
    public class PostImage
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("post_id")]
        [ForeignKey(nameof(Post))]
        public Guid PostId { get; set; }


        [Column("image_url")]
        public required string ImageUrl { get; set; }

        public Post? Post { get; set; }
    }
}

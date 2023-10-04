using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShareSpace.Server.Entities
{
    [Table("tags")]
    public class Tag
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("name")]
        public required string Name { get; set; }
    }
}

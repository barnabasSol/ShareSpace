using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShareSpace.Server.Entities;

[Table("notification_types")]
public class NotificationType
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public required string Name { get; set; }
}

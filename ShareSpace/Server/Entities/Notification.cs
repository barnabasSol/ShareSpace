using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShareSpace.Server.Entities
{
    [Table("notifications")]
    public class Notification
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("seen")]
        public bool Seen { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("user_id")]
        [ForeignKey("GetterUser")]
        public Guid UserId { get; set; }

        [Column("source_id")]
        [ForeignKey("UserSource")]
        public Guid SourceId { get; set; }

        [Column("type")]
        [ForeignKey("NotificationType")]
        public int Type { get; set; }

        public User? GetterUser { get; set; }
        public User? UserSource { get; set; }
        public NotificationType? NotificationType { get; set; }
    }
}

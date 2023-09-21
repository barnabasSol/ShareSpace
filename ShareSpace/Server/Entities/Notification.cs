using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShareSpace.Server.Entities
{
    public class Notification
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("user_id")]
        public Guid UserId { get; set; }
        [Column("source_id")]
        public Guid SourceId { get; set; }
        [Column("notification_type")]
        public int Type { get; set; }
        [Column("seen")]
        public bool Seen { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

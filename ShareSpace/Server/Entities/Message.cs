using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShareSpace.Server.Entities
{
    public class Message
    {
        [Key]
        [Column("message_id")]
        public int MessageId { get; set; }
        [Column("content")]
        public required string Content { get; set; }
        [Column("seen")]
        public bool Seen {  get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Column("sender_id")]
        public Guid SenderId { get; set; }
        [Column("sender_id")]
        public Guid ReceiverId { get; set; }
    }
}

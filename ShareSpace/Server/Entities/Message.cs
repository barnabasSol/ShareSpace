using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShareSpace.Server.Entities
{
    [Table("messages")]
    public class Message
    {
        [Key]
        [Column("message_id")]
        public Guid MessageId { get; set; }

        [Column("content")]
        public required string Content { get; set; }

        [Column("seen")]
        public bool Seen { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("sender_id")]
        [ForeignKey("Sender")]
        public Guid SenderId { get; set; }

        [Column("receiver_id")]
        [ForeignKey("Receiver")]
        public Guid ReceiverId { get; set; }

        public User? Receiver { get; set; }
        public User? Sender { get; set; }
    }
}

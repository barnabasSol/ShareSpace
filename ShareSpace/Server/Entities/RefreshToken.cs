using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShareSpace.Server.Entities
{
    [Table("refresh_token")]
    public class RefreshToken
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("expiration_date")]
        public DateTime ExpirationDate { get; set; }

        [Column("token")]
        public string Token { get; set; } = string.Empty;

        [Column("user_id")]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        public User? User { get; set; }
    }
}

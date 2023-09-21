using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShareSpace.Server.Entities
{
    public class User
    {
        [Key]
        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("username", TypeName = "VARCHAR(100)")]
        public required string UserName { get; set; }

        [Column("first_name", TypeName = "VARCHAR(100)")]
        public required string FirstName { get; set; }

        [Column("last_name", TypeName = "VARCHAR(100)")]
        public string? LastName { get; set; }

        [Column("email", TypeName = "VARCHAR(150)")]
        public required string Email { get; set; }

        [Column("password", TypeName = "bytea")]
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();

        [Column("bio")]
        public string? Bio { get; set; }

        [Column("profile_pic_url")]
        public string? ProfilePicUrl { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace ShareSpace.Server.Entities;

[Table("user_role")]
public class UserRole
{
    [Column("user_id")]
    [ForeignKey("User")]
    public Guid UserId { get; set; }

    [Column("role_id")]
    [ForeignKey("Role")]
    public int RoleId { get; set; }

    public virtual Role? Role { get; set; }
    public virtual User? User { get; set; }
}

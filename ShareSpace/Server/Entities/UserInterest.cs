using System.ComponentModel.DataAnnotations.Schema;

namespace ShareSpace.Server.Entities
{
    [Table("user_interests")]
    public class UserInterest
    {

        [Column("user_id")]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [Column("interest_id")]
        [ForeignKey("Interest")]
        public int InterestId { get; set; }

        public Interest? Interest { get; set; }
        public User? User { get; set; }

    }
}

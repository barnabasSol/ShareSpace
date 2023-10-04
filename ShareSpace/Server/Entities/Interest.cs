using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShareSpace.Server.Entities
{
    [Table("interests")]
    public class Interest
    {
        [Key]
        [Column("interest_id")]
        public int InterestId { get; set; }
        
        [Column("interest_name")]
        public string InterestName { get; set; } = string.Empty;
    }
}
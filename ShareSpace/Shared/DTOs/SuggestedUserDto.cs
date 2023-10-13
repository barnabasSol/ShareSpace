namespace ShareSpace.Shared.DTOs
{
    public class SuggestedUserDto{
       public required string Name { get; set; }
       public required string UserName {get; set; }
       public string? ProfilePicUrl { get; set; }
       public required IEnumerable<InterestsDto> InterestsDtos {get; set; }
    }
}

namespace ShareSpace.Shared.DTOs
{
    public class SuggestedUserDto
    {
        public required string Name { get; set; }
        public string? ProfilePicUrl { get; set; }
        public string UserName { get; set; } = string.Empty;
        public Guid UserId { get; set; }
    }
}

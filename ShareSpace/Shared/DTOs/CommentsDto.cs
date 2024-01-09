namespace ShareSpace.Shared.DTOs;

public class CommentDto
{
    public Guid UserId { get; set; }
    public required string UserName { get; set; }
    public required string Name { get; set; }
    public string? UserProfilePicUrl { get; set; }
    public required string Content { get; set; }
}

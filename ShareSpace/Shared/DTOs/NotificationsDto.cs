namespace ShareSpace.Shared.DTOs;

public enum Status
{
    Followed = 1,
    Unfollowed = -1
}

public class NotificationsDto
{
    public required string UserName { get; set; }
    public string? Name { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? ProfilePicUrl { get; set; }
}

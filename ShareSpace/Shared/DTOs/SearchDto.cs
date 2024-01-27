namespace ShareSpace.Shared.DTOs;

public class UserSearchDto
{
    public Guid UserId { get; set; }
    public string? ProfilePic { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsBeingFollowed { get; set; }
}

public class PostSearchDto : PostBase
{
    public string? PostUserProfilePicUrl { get; set; }
    public string PostedName { get; set; } = string.Empty;
    public string? TextContent { get; set; }
    public IEnumerable<string>? PostPictureUrls { get; set; }
    public int LikesCount { get; set; }
    public int ViewsCount { get; set; }
    public int CommentsCount { get; set; }
    public DateTime PostedDateTime { get; set; }
    public bool IsLikedByCurrentUser { get; set; }
}

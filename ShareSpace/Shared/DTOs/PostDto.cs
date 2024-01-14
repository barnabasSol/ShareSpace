namespace ShareSpace.Shared.DTOs;

public class PostBase
{
    public Guid PostId { get; set; }
    public required Guid PostedUserId { get; set; }
    public required string PostedUsername { get; set; }
}

public class PostDto : PostBase
{
    public string? PostUserProfilePicUrl { get; set; }
    public required string PostedName { get; set; }
    public string? TextContent { get; set; }
    public IEnumerable<string>? PostPictureUrls { get; set; }
    public int LikesCount { get; set; }
    public int ViewsCount { get; set; }
    public int CommentsCount { get; set; }
    public DateTime PostedDateTime { get; set; }
    public bool IsLikedByCurrentUser { get; set; }
}

public class PostDetailDto : PostDto
{
    public List<CommentDto>? Comments { get; set; }
}

public class CreatePostDto
{
    public string? TextContent { get; set; }
    public IEnumerable<File>? PostFiles { get; set; }
    public Guid PostedUserId { get; set; }
    public IEnumerable<string>? Tags { get; set; }
}

public class File
{
    public byte[]? ImageBytes { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public long Size { get; set; }
}

public class LikedPostDto
{
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
}

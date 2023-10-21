namespace ShareSpace.Shared.DTOs
{
    public class PostBase
    {
        public Guid PostId { get; set; }
        public required string PostedUserId { get; set; }
        public required string PostUsername { get; set; }
    }

    public class PostDto : PostBase
    {
        public string? PostUserProfilePicUrl { get; set; }
        public string? TextContent { get; set; }
        public IEnumerable<string>? PostPictureUrls { get; set; }
        public int LikesCount { get; set; }
        public int ViewsCount { get; set; }
        public int CommentsCount { get; set; }
    }

    public class PostDtoDetailsDto : PostDto { }

    public class CreatePostDto
    {
        public string? TextContent { get; set; }
        public IEnumerable<PostFile>? PostFiles { get; set; }
        public Guid PostedUserId { get; set; }
    }

    public class PostFile
    {
        public byte[]? ImageBytes { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public long Size { get; set; }
    }

}

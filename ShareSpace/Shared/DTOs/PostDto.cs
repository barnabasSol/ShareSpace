namespace ShareSpace.Shared.DTOs
{
    public class PostDto
    {
        public Guid PostId { get; set; }
        public required string PostedUser { get; set; }
        public required string PostUsername { get; set; }
        public required string PostUserProfilePicUrl { get; set; }
        public string TextContent { get; set; } = string.Empty;
        public string? PostPicture { get; set; } = string.Empty;
        public int LikesCount { get; set; }
        public int CommentCount { get; set; }
    }

    public class PostDtoDetails : PostDto { }
}

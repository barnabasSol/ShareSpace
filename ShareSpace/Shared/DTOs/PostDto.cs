namespace ShareSpace.Shared.DTOs
{
    public class PostDto
    {
        public Guid PostId { get; set; }
        public required string PostedUser { get; set; }
        public required string PostUsername { get; set; }
        public required string PostUserProfilePicUrl { get; set; }
        public string TextContent { get; set; } = string.Empty;
        public string? PostPicture { get; set; } 
        public int LikesCount { get; set; }
        public int ViewsCount { get; set; }
        public int CommentsCount { get; set; }
    }

    public class PostDtoDetailsDto : PostDto { }
}

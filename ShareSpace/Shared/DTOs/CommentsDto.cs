namespace ShareSpace.Shared.DTOs
{
    public class CommentDto
    {
        public Guid CommentedUserId { get; set; }
        public required string UserName { get; set; }
        public string? CommentedUserProfilePicUrl { get; set; }
        public required string CommentContent { get; set; }
    }
}

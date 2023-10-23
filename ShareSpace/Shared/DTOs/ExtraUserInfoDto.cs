namespace ShareSpace.Shared.DTOs
{
    public class ExtraUserInfoDto
    {
        public string? ProfilePicUrl { get; set; }
        public string? Bio { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
        public DateTime JoinedDate { get; set; }
    }
}

namespace ShareSpace.Shared.DTOs;

public class CreateUserDTO
{
    public required string Name { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class ExtraUserInfoDto
{
    public string? ProfilePicUrl { get; set; }
    public string? Bio { get; set; }
    public int FollowersCount { get; set; }
    public int FollowingCount { get; set; }
    public DateTime JoinedDate { get; set; }
    public IEnumerable<InterestsDto>? Interests { get; set; }
}

public class UserLoginDTO
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
}
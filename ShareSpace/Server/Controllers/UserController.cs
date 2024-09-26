using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareSpace.Server.Repository.Contracts;
using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Server.Controllers;

[ApiController]
[Authorize(Roles = "user, admin")]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository userRepository;

    public UserController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    [HttpGet("interests")]
    public async Task<ActionResult<ApiResponse<IEnumerable<InterestsDto>>>> GetInterests()
    {
        try
        {
            var response = await userRepository.GetInterests();
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ApiResponse<string>
                {
                    IsSuccess = false,
                    Message = $"server error happened, {ex.Message}. try again later"
                }
            );
        }
    }

    [HttpPost("store-interests")]
    public async Task<ActionResult<ApiResponse<string>>> StoreInterests(
        IEnumerable<InterestsDto> interests
    )
    {
        try
        {
            var result = await userRepository.StoreInterests(
                current_user: Guid.Parse(User.FindFirst("Sub")!.Value),
                interests: interests
            );
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ApiResponse<string>
                {
                    IsSuccess = false,
                    Message = $"server error happened, {ex.Message}. try again later"
                }
            );
        }
    }

    [HttpGet("extra-info/{userid}")]
    public async Task<ActionResult<ApiResponse<ExtraUserInfoDto>>> GetExtraInfo(Guid userid)
    {
        try
        {
            var result = await userRepository.GetExtraUserInfo(userid);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ApiResponse<string>
                {
                    IsSuccess = false,
                    Message = $"server error happened, {ex.Message}. try again later"
                }
            );
        }
    }

    [HttpGet("profile/{username}")]
    public async Task<ActionResult<ApiResponse<ProfileDto>>> GetProfile(string username)
    {
        try
        {
            Guid UserId = Guid.Parse(User.FindFirst("Sub")!.Value);
            var response = await userRepository.GetProfile(username, UserId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ApiResponse<string>
                {
                    IsSuccess = false,
                    Message = $"server error happened, {ex.Message}. try again later"
                }
            );
        }
    }

    [HttpPut("follow/{userid}")]
    public async Task<ActionResult<ApiResponse<string>>> FollowUser(Guid userid)
    {
        try
        {
            Guid UserId = Guid.Parse(User.FindFirst("Sub")!.Value);
            var result = await userRepository.FollowUser(followed_id: userid, follower_id: UserId);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ApiResponse<string>
                {
                    IsSuccess = false,
                    Message = $"server error happened, {ex.Message}. try again later"
                }
            );
        }
    }

    [HttpGet("suggested-users")]
    public async Task<ActionResult<ApiResponse<ExtraUserInfoDto>>> GetSuggestedUsers()
    {
        try
        {
            Guid UserId = Guid.Parse(User.FindFirst("Sub")!.Value);
            var result = await userRepository.GetSuggestedUsers(UserId);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ApiResponse<string>
                {
                    IsSuccess = false,
                    Message = $"server error happened, {ex.Message}. try again later"
                }
            );
        }
    }

    [HttpGet("followers")]
    public async Task<ActionResult<ApiResponse<IEnumerable<FollowerUserDto>>>> GetFollowers()
    {
        try
        {
            Guid UserId = Guid.Parse(User.FindFirst("Sub")!.Value);
            var result = await userRepository.GetFollowers(UserId);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ApiResponse<string>
                {
                    IsSuccess = false,
                    Message = $"server error happened, {ex.Message}. try again later"
                }
            );
        }
    }

    [HttpGet("following")]
    public async Task<ActionResult<ApiResponse<IEnumerable<FollowerUserDto>>>> GetFollowing()
    {
        try
        {
            Guid UserId = Guid.Parse(User.FindFirst("Sub")!.Value);
            var result = await userRepository.GetFollowing(UserId);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ApiResponse<string>
                {
                    IsSuccess = false,
                    Message = $"server error happened, {ex.Message}. try again later"
                }
            );
        }
    }
}

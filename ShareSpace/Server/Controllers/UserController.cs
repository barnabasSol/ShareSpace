using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareSpace.Server.Repository.Contracts;
using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Server.Controllers;

[ApiController]
[Authorize(Roles = "user")]
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
            return new ApiResponse<IEnumerable<InterestsDto>>
            {
                IsSuccess = false,
                Message = $"a server error occured {ex.Message}",
                Data = Enumerable.Empty<InterestsDto>()
            };
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
            return new ApiResponse<string>
            {
                IsSuccess = false,
                Message = $"a server error occured {ex.Message}",
                Data = ""
            };
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
            return new ApiResponse<ExtraUserInfoDto>
            {
                IsSuccess = false,
                Message = $"a server error occured {ex.Message}",
            };
        }
    }

    [HttpGet("profile/{userid}")]
    public async Task<ActionResult<ApiResponse<ProfileDto>>> GetProfile(Guid userid)
    {
        try
        {
            Guid UserId = Guid.Parse(User.FindFirst("Sub")!.Value);
            var response = await userRepository.GetProfile(userid, UserId);
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
            return new ApiResponse<string>
            {
                IsSuccess = false,
                Message = $"a server error occured {ex.Message}",
            };
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
            return new ApiResponse<ExtraUserInfoDto>
            {
                IsSuccess = false,
                Message = $"a server error occured {ex.Message}",
            };
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
            return new ApiResponse<IEnumerable<FollowerUserDto>>
            {
                IsSuccess = false,
                Message = $"a server error occured, {ex.Message}",
                Data = Enumerable.Empty<FollowerUserDto>()
            };
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
            return new ApiResponse<IEnumerable<FollowerUserDto>>
            {
                IsSuccess = false,
                Message = $"a server error occured {ex.Message}",
                Data = Enumerable.Empty<FollowerUserDto>()
            };
        }
    }
}

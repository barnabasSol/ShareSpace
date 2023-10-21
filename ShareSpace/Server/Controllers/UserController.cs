﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareSpace.Server.Repository.Contracts;
using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [Authorize]
        [HttpGet("get-interests")]
        public async Task<ActionResult<ApiResponse<IEnumerable<InterestsDto>>>> GetInterests()
        {
            try
            {
                var interests = await userRepository.GetInterests();
                return interests.IsSuccess ? Ok(interests) : BadRequest(interests);
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<InterestsDto>>()
                {
                    IsSuccess = false,
                    Message = $"a server error occured {ex.Message}",
                    Data = Enumerable.Empty<InterestsDto>()
                };
            }
        }

        [Authorize]
        [HttpPost("store-interests")]
        public async Task<ActionResult<ApiResponse<string>>> StoreInterests(
            IEnumerable<InterestsDto> interests
        )
        {
            string UserId = User.FindFirst("Sub")!.Value;
            try
            {
                var result = await userRepository.StoreInterests(
                    current_user: Guid.Parse(UserId),
                    interests: interests
                );
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>()
                {
                    IsSuccess = false,
                    Message = $"a server error occured {ex.Message}",
                    Data = ""
                };
            }
        }

        [Authorize]
        [HttpPost("get-extra-user-info")]
        public async Task<ActionResult<ApiResponse<ExtraUserInfoDto>>> GetExtraInfo()
        {
            string UserId = User.FindFirst("Sub")!.Value;
            try
            {
                var result = await userRepository.GetExtraUserInfo(Guid.Parse(UserId));
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ExtraUserInfoDto>()
                {
                    IsSuccess = false,
                    Message = $"a server error occured {ex.Message}",
                };
            }
        }
    }
}
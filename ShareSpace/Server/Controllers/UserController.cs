using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult<DataResponse<IEnumerable<InterestsDto>>>> GetInterests()
        {
            return await userRepository.GetInterests();
        }

        [Authorize]
        [HttpPost("store-interests")]
        public async Task<ActionResult<DataResponse<string>>> StoreInterests(IEnumerable<InterestsDto> interests)
        {
            string UserId = User.FindFirst("Sub")!.Value;
            await Console.Out.WriteLineAsync(UserId);
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
                return new DataResponse<string>()
                {
                    IsSuccess = false,
                    Message = $"a server error occured {ex.Message}"
                };
            }
        }
    }
}

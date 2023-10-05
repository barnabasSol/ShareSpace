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

        [HttpGet("get-interests")]
        [Authorize]
        public async Task<ActionResult<DataResponse<IEnumerable<InterestsDto>>>> GetInterests()
        {
            // string UserId = User.FindFirst("Sub")!.Value;
            return await userRepository.GetInterests();
        }
    }
}

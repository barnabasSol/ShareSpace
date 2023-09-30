using Microsoft.AspNetCore.Mvc;
using ShareSpace.Server.Repository.Contracts;
using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public AuthController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpPost("create-user")]
        public async Task<ActionResult<CreateUserResponse>> CreateUser(CreateUserDTO user)
        {
            try
            {
                var response = await userRepository.CreateUser(user);
                return response.IsCreated ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new CreateUserResponse() { IsCreated = false, Message = ex.Message }
                );
            }
        }

        [HttpPost("login-user")]
        public async Task<ActionResult<LoginResponse>> LoginUser(UserLoginDTO user)
        {
            try
            {
                var response = await userRepository.LoginUser(user);
                return response.Authorized is true ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new LoginResponse() { Authorized = false, Message = ex.Message }
                );
            }
        }
    }
}

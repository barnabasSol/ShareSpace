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
        private readonly IAuthRepository authRepository;

        public AuthController(IAuthRepository userRepository)
        {
            this.authRepository = userRepository;
        }

        [HttpPost("create-user")]
        public async Task<ActionResult<AuthResponse>> CreateUser(CreateUserDTO user)
        {
            try
            {
                var response = await authRepository.CreateUser(user);
                return response.IsSuccess ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new AuthResponse() { IsSuccess = false, Message = ex.Message }
                );
            }
        }

        [HttpPost("login-user")]
        public async Task<ActionResult<AuthResponse>> LoginUser(UserLoginDTO user)
        {
            try
            {
                var response = await authRepository.LoginUser(user);
                return response.IsSuccess is true ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new AuthResponse() { IsSuccess = false, Message = ex.Message }
                );
            }
        }
    }
}

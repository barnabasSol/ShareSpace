using Microsoft.AspNetCore.Mvc;
using ShareSpace.Server.Repository.Contracts;
using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Server.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthRepository authRepository;

    public AuthController(IAuthRepository authRepository)
    {
        this.authRepository = authRepository;
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
                new AuthResponse()
                {
                    IsSuccess = false,
                    Message = $"server error happened, {ex.Message}. try again later"
                }
            );
        }
    }

    [HttpPost("login-user")]
    public async Task<ActionResult<AuthResponse>> LoginUser(
        UserLoginDTO user,
        [FromServices] IAuthRepository authRepository
    )
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
                new AuthResponse()
                {
                    IsSuccess = false,
                    Message = "something went wrong, try again later." + ex.Message
                }
            );
        }
    }
}

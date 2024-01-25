using Microsoft.AspNetCore.Mvc;
using ShareSpace.Server.Repository.Contracts;
using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Server.Controllers;

[Route("[controller]")]
[ApiController]
public class SettingsController : ControllerBase
{
    private readonly ISettingsRepository settingsRepository;

    public SettingsController(ISettingsRepository settingsRepository)
    {
        this.settingsRepository = settingsRepository;
    }

    [HttpPut("update-profile")]
    public async Task<ActionResult<AuthResponse>> GetNotifications(
        UpdateUserProfileDto updateUserProfileDto
    )
    {
        try
        {
            Guid UserId = Guid.Parse(User.FindFirst("Sub")!.Value);
            var response = await settingsRepository.UpdateProfile(updateUserProfileDto, UserId);
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
}

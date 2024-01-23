using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareSpace.Server.Repository.Contracts;
using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Server.Controllers;

[Route("[controller]")]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly INotificationRepostiory notificationRepostiory;

    public NotificationController(INotificationRepostiory notificationRepostiory)
    {
        this.notificationRepostiory = notificationRepostiory;
    }

    [HttpGet("notifications")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<ApiResponse<NotificationsDto>>> GetNotifications()
    {
        try
        {
            Guid UserId = Guid.Parse(User.FindFirst("Sub")!.Value);
            var response = await notificationRepostiory.GetNotifications(UserId);
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
    [HttpGet("count")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<int>> GetNotificationsCount()
    {
        try
        {
            Guid UserId = Guid.Parse(User.FindFirst("Sub")!.Value);
            var response = await notificationRepostiory.GetNotificationCount(UserId);
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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareSpace.Server.Repository.Contracts;
using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository messageRepository;

        public MessageController(IMessageRepository messageRepository)
        {
            this.messageRepository = messageRepository;
        }

        // [Authorize]
        // [HttpPost("store-message")]
        // public async Task<ActionResult<ApiResponse<string>>> StoreMessage(MessageDto messageDto)
        // {
        //     try
        //     {
        //         var response = await messageRepository.StoreMessage(messageDto);
        //         return response.IsSuccess ? Ok(response) : BadRequest(response);
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(
        //             StatusCodes.Status500InternalServerError,
        //             new ApiResponse<string>()
        //             {
        //                 IsSuccess = false,
        //                 Message = $"server error happened, {ex.Message}. try again later"
        //             }
        //         );
        //     }
        // }

        [Authorize]
        [HttpGet("get-messages")]
        public async Task<ActionResult<ApiResponse<string>>> GetMessages()
        {
            try
            {
                Guid current_user = Guid.Parse(User.FindFirst("Sub")!.Value);
                var response = await messageRepository.GetMessagesOfUser(current_user);
                return response.IsSuccess ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>()
                    {
                        IsSuccess = false,
                        Message = $"server error happened, {ex.Message}. try again later"
                    }
                );
            }
        }

        // [Authorize]
        // [HttpGet("users-chat")]
        // public async Task<ActionResult<ApiResponse<string>>> GetUsersInChat()
        // {
        //     try
        //     {
        //         Guid current_user = Guid.Parse(User.FindFirst("Sub")!.Value);
        //         var response = await messageRepository.GetUsersInChat(current_user);
        //         return response.IsSuccess ? Ok(response) : BadRequest(response);
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(
        //             StatusCodes.Status500InternalServerError,
        //             new ApiResponse<string>()
        //             {
        //                 IsSuccess = false,
        //                 Message = $"server error happened, {ex.Message}. try again later"
        //             }
        //         );
        //     }
        // }
    }
}

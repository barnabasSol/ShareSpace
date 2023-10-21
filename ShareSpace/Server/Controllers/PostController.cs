using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareSpace.Server.Entities;
using ShareSpace.Server.Repository.Contracts;
using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;
using System;

namespace ShareSpace.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository postRepository;

        public PostController(IPostRepository postRepository)
        {
            this.postRepository = postRepository;
        }

        [HttpPost("create-post")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<string>>> CreatePost(CreatePostDto post)
        {
            try
            {
                string UserId = User.FindFirst("Sub")!.Value;
                post.PostedUserId = Guid.Parse(UserId);
                var response = await postRepository.CreatePost(post);
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
    

   }
}

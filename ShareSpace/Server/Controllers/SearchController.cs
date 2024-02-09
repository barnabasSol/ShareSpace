using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareSpace.Server.Repository.Contracts;
using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Server.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize(Roles = "user")]
public class SearchController : ControllerBase
{
    private readonly ISearchRepository searchRepository;

    public SearchController(ISearchRepository searchRepository)
    {
        this.searchRepository = searchRepository;
    }

    [HttpGet("users/{query}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<UserSearchDto>>>> SearchUsers(
        string query
    )
    {
        try
        {
            Guid UserId = Guid.Parse(User.FindFirst("Sub")!.Value);
            var response = await searchRepository.SearchUser(query.ToLower(), UserId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    IsSuccess = false,
                    Message = $"server error happened, {ex.Message}. try again later"
                }
            );
        }
    }

    [HttpGet("posts/{query}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<PostSearchDto>>>> SearchPosts(
        string query
    )
    {
        try
        {
            Guid UserId = Guid.Parse(User.FindFirst("Sub")!.Value);
            var response = await searchRepository.SearchPost(query.ToLower(), UserId);
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

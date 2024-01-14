using Microsoft.AspNetCore.Mvc;
using ShareSpace.Server.Repository.Contracts;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Server.Controllers;

[Route("[controller]")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository commentRepository;

    public CommentController(ICommentRepository commentRepository)
    {
        this.commentRepository = commentRepository;
    }

    [HttpPost("add")]
    public Task<ActionResult<AuthResponse>> Add()
    {
        throw new NotImplementedException();
    }

    [HttpDelete("delete")]
    public Task<ActionResult<AuthResponse>> Delete()
    {
        throw new NotImplementedException();
    }
}

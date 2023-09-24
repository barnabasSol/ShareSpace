using Microsoft.AspNetCore.Mvc;

namespace ShareSpace.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return await Task.FromResult<ActionResult>(Ok("fuck"));
        }
    }
}

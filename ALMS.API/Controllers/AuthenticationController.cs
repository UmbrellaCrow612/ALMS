using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ALMS.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register()
        {
            return Ok();
        }
    }
}

using KA.Application.DTO;
using KA.Application.Interfaces;
using KAApi.Common;
using Microsoft.AspNetCore.Mvc;

namespace KA.Api1.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _users;
        private readonly IConfiguration _configuration;
        public AuthController(IUserService userService, IConfiguration configuration)
        {
            _users = userService;
            _configuration= configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserDTO userR)
        {
            var user = _users.ValidateUserAsync(userR.Username, userR.Password);
            if (user.Result == null)
            {
                return Unauthorized("Invalid credentials.");
            }
            var token = AuthenticatedHelper.GenerateJwtToken(userR.Username, _configuration);
            return Ok(new { token });
        }
    }
}

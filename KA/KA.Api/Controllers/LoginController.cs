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
        /// <summary>
        /// Validate if user exist in DB and compare teh data inserted and data in DB
        /// </summary>
        /// <param name="userR">Data from frontend</param>
        /// <returns></returns>
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserDTO userR)
        {
            var user = _users.ValidateUserAsync(userR.Username, userR.Password);
            if (user.Result == null)
            {
                return Unauthorized("Invalid credentials.");
            }
            //Create a JWT token to authentication
            var token = AuthenticatedHelper.GenerateJwtToken(userR.Username, _configuration);
            return Ok(new { token });
        }
    }
}

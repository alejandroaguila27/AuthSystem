using AuthSystem.API.Models;
using AuthSystem.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthSystem.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IJwtService _jwtService;

        public AuthController(UserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticateDto authenticateDto)
        {
            var token = await _userService.AuthenticateUser(authenticateDto.Username, authenticateDto.Password);

            if (token == null)
            {
                var response = new
                {
                    Message = "Credenciales no válidas"
                };
                return Unauthorized(response);
            }

            return Ok(new { Token = token, Expiration = DateTime.UtcNow.AddMinutes(_jwtService.ExpirationInMinutes) });
        }

    }
}

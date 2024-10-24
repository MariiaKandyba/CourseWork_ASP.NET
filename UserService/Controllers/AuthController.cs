using DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserServiceApi.Services;

namespace UserServiceApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var result = await _userService.RegisterAsync(registerDto);
            if (result.IsSuccess)
            {
                return Ok(new { result.Token });
            }
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _userService.LoginAsync(loginDto);
            if (result.IsSuccess)
            {
                return Ok(new { result.Token });
            }
            return Unauthorized(result.ErrorMessage);
        }
    }
}
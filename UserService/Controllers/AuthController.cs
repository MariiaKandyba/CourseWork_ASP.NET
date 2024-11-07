using DTOs.Admin;
using DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
                return Ok(new AuthResultDto
                {
                    IsSuccess = true,
                    Token = result.Token
                });
            }
            return Unauthorized(new AuthResultDto
            {
                IsSuccess = false,
                ErrorMessage = result.ErrorMessage
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _userService.LoginAsync(loginDto);

            if (result.IsSuccess)
            {
                return Ok(new AuthResultDto
                {
                    IsSuccess = true,
                    Token = result.Token
                });
            }

            return Unauthorized(new AuthResultDto
            {
                IsSuccess = false,
                ErrorMessage = result.ErrorMessage
            });
        }


        [HttpPut("updateProfile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateDto updateProfileDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await _userService.UpdateProfileAsync(userId, updateProfileDto);
            if (result.IsSuccess)
            {
                return Ok(new AuthResultDto
                {
                    IsSuccess = true,
                    Token = result.Token
                });
            }

            return Unauthorized(new AuthResultDto
            {
                IsSuccess = false,
                ErrorMessage = result.ErrorMessage
            });
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllUsersAsync();
            if (result == null || !result.Any())
            {
                return NotFound("No users found.");
            }
            
            return Ok(result);
        }

    }
}
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
        private readonly IUserManagementService _userManagementService;
        private readonly IAuthenticationService _authenticationService;
        public UserController(IUserManagementService userManagementService, IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _userManagementService = userManagementService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var result = await _authenticationService.RegisterAsync(registerDto);
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
            var result = await _authenticationService.LoginAsync(loginDto);

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
            var result = await _authenticationService.UpdateProfileAsync(userId, updateProfileDto);
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userManagementService.GetAllUsersAsync();
            if (result == null || !result.Any())
            {
                return NotFound("No users found.");
            }
            
            return Ok(result);
        }
        [HttpPost("create")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto registerDto)
        {
            var result = await _userManagementService.CreateUserAsync(registerDto);
            if (result.IsSuccess)
            {
                return Ok(new AuthResultDto
                {
                    IsSuccess = true,
                });
            }

            return BadRequest(new AuthResultDto
            {
                IsSuccess = false,
                ErrorMessage = result.ErrorMessage
            });
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userManagementService.DeleteUserAsync(id);
            if (result.IsSuccess)
            {
                return NoContent(); 
            }

            return NotFound(new
            {
                IsSuccess = false,
                ErrorMessage = "User not found"
            }); 
        }


    }
}
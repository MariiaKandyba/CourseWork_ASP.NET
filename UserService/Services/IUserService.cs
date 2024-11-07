using DTOs.Admin;
using DTOs.Auth;

namespace UserServiceApi.Services
{
    public interface IUserService
    {
        Task<AuthResultDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResultDto> LoginAsync(LoginDto loginDto);
        Task<AuthResultDto> UpdateProfileAsync(int userId, UpdateDto updateDto);
        Task<List<UserDto>> GetAllUsersAsync();  
        Task<UserDto> GetUserByIdAsync(int userId);  
    }

}

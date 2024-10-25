using DTOs.Auth;
using UserServiceApi.Models;

namespace UserServiceApi.Services
{
    public interface IUserService
    {
        Task<AuthResultDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResultDto> LoginAsync(LoginDto loginDto);
        Task<AuthResultDto> UpdateProfileAsync(int userId, UpdateDto updateDto);

    }

}

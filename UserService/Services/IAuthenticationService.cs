using DTOs.Auth;

namespace UserServiceApi.Services
{
    public interface IAuthenticationService
    {
        Task<AuthResultDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResultDto> LoginAsync(LoginDto loginDto);
        Task<AuthResultDto> UpdateProfileAsync(int userId, UpdateDto updateDto);
    }
}

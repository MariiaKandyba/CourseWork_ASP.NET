using DTOs.Auth;

namespace UserServiceApi.Services
{
    public interface IUserService
    {
        Task<AuthResultDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResultDto> LoginAsync(LoginDto loginDto);
    }

}

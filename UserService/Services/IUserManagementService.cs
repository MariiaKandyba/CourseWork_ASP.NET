using DTOs.Admin;
using DTOs.Auth;

namespace UserServiceApi.Services
{
    public interface IUserManagementService
    {
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(int userId);
        Task<AuthResultDto> CreateUserAsync(CreateUserDto createUserDto);
        Task<AuthResultDto> DeleteUserAsync(int userId);
    }
}

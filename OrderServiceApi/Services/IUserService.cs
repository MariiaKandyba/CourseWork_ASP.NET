using DTOs.Admin;

namespace OrderServiceApi.Services
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsersAsync(string token);
    }
}

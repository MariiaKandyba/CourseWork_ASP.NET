using UserServiceApi.Models;

namespace UserServiceApi.Services
{
    public interface IJwtTokenService
    {
        string GenerateJwtToken(User user);

    }
}

using DTOs.Admin;
using DTOs.Auth;
using DTOs.Orders;

namespace Client.Areas.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<UserDto>> GetAll()
        {
            var response = await _httpClient.GetAsync("https://localhost:7140/gateway/users/all");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<UserDto>>();

        }

    }
}

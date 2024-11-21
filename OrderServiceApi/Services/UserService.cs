using DTOs.Admin;
using DTOs.Orders;

namespace OrderServiceApi.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<UserDto>> GetAllUsersAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"https://localhost:7140/gateway/users/all");
            return await response.Content.ReadFromJsonAsync<List<UserDto>>();
        }
    }
}

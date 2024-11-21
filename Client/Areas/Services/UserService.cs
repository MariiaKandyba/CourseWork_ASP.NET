using Client.Config;
using DTOs.Admin;
using DTOs.Auth;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Client.Areas.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiGatewayConfig _config;

        public UserService(HttpClient httpClient, ApiGatewayConfig config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<List<UserDto>> GetAll(string token)
        {
            SetAuthorizationHeader(token);
            var response = await _httpClient.GetAsync($"{_config.BaseUrl}{_config.UsersAll}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<UserDto>>();
        }

        public async Task<bool> AddUser(CreateUserDto createUserDto, string token)
        {
            SetAuthorizationHeader(token);
            var response = await _httpClient.PostAsJsonAsync($"{_config.BaseUrl}{_config.UsersCreate}", createUserDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUser(int userId, string token)
        {
            SetAuthorizationHeader(token);
            var response = await _httpClient.DeleteAsync($"{_config.BaseUrl}{string.Format(_config.UserById, userId)}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to delete user: {error}");
        }

        private void SetAuthorizationHeader(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}

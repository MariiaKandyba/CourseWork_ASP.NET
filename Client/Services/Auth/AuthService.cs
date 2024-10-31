using DTOs.Auth;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Client.Services.Auth
{

    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AuthResultDto> RegisterAsync(RegisterDto model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("https://localhost:7140/gateway/users/register", content);

                var resultContent = await response.Content.ReadAsStringAsync();
                var authResult = JsonConvert.DeserializeObject<AuthResultDto>(resultContent);

                return authResult;
            }
            catch (HttpRequestException ex)
            {
                return new AuthResultDto
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<AuthResultDto> LoginAsync(LoginDto model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("https://localhost:7140/gateway/users/login", content);

                var resultContent = await response.Content.ReadAsStringAsync();
                var authResult = JsonConvert.DeserializeObject<AuthResultDto>(resultContent);

                return authResult;
            }
            catch (HttpRequestException ex)
            {
                return new AuthResultDto
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }
        public async Task<AuthResultDto> UpdateProfileAsync(UpdateDto updateProfileDto, string token)
        {
            var json = JsonConvert.SerializeObject(updateProfileDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await _httpClient.PutAsync("https://localhost:7140/gateway/users/updateProfile", content);
                

                if (response.IsSuccessStatusCode)
                {
                    var resultContent = await response.Content.ReadAsStringAsync();
                    var authResult = JsonConvert.DeserializeObject<AuthResultDto>(resultContent);
                    return new AuthResultDto { IsSuccess = true, Token = authResult.Token };
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                return new AuthResultDto { IsSuccess = false, ErrorMessage = errorContent };
            }
            catch (Exception ex)
            {
                return new AuthResultDto { IsSuccess = false, ErrorMessage = ex.Message };
            }
        }


    }
}

using DTOs.Admin;

namespace Client.Areas.Services
{
    public class StockProductService
    {
        private readonly HttpClient _httpClient;
        public StockProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<FullProductDto>> GetAll()
        {
            var response = await _httpClient.GetAsync("https://localhost:7140/gateway/products/full");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<FullProductDto>>();

        }
    }
}

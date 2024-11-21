using DTOs.Admin;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using DTOs.Products;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;

namespace Client.Areas.Services
{
    public class StockProductService
    {
        private readonly HttpClient _httpClient;

        public StockProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<FullProductDto>> GetAll(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await _httpClient.GetAsync("https://localhost:7140/gateway/products/full");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<FullProductDto>>();
        }
        public async Task<List<CategoryDto>> GetCategories()
        {
            var response = await _httpClient.GetAsync("https://localhost:7140/gateway/category");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<CategoryDto>>();
        }
        public async Task<List<BrandDto>> GetBrands()
        {
            var response = await _httpClient.GetAsync("https://localhost:7140/gateway/brand");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<BrandDto>>();
        }

        public async Task<FullProductDto> GetById(int id, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await _httpClient.GetAsync($"https://localhost:7140/gateway/products/full/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<FullProductDto>();
        }

        public async Task Create(ProductCreateDto productDto, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7140/gateway/products", productDto);
            response.EnsureSuccessStatusCode();
        }

        public async Task Update(ProductEditDto productDto, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await _httpClient.PutAsJsonAsync($"https://localhost:7140/gateway/products/{productDto.Id}", productDto);
            response.EnsureSuccessStatusCode();
        }

        public async Task Delete(int id, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await _httpClient.DeleteAsync($"https://localhost:7140/gateway/products/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}

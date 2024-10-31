using DTOs.Products;

namespace Client.Services.Products
{
    public class ProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProductDto>> GetProductsAsync()
        {
            var response = await _httpClient.GetAsync("https://localhost:7140/gateway/products");
            return await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        }
        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7140/gateway/products/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ProductDto>();
        }
        public async Task<List<ProductDto>> GetProductsByIdsAsync(List<int> productIds)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7140/gateway/products/batch", productIds);
            return await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        }

    }
}

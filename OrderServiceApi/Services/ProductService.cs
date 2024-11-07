using DTOs.Products;

namespace OrderServiceApi.Services
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
        public async Task<List<ProductDto>> GetPaginatedProductsAsync(int page, int pageSize, List<CategoryDto> сategories)
        {
            var url = $"https://localhost:7140/gateway/products/paginated?page={page}&pageSize={pageSize}";

            if (сategories != null && сategories.Any())
            {
                var categories = string.Join(",", сategories.Select(c => c.Name));
                url += $"&categories={categories}";
            }

            var response = await _httpClient.GetAsync(url);

            return await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        }


        public async Task<int> GetTotalProductCountAsync(List<CategoryDto> categories)
        {
            var url = "https://localhost:7140/gateway/products/count";

            if (categories != null && categories.Any())
            {
                var categories2 = string.Join(",", categories.Select(c => c.Name));
                url += $"?categories={categories2}";
            }

            var response = await _httpClient.GetAsync(url);
            return await response.Content.ReadFromJsonAsync<int>();
        }



    }

}

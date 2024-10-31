using DTOs.Orders;
using DTOs.Products;
using System.Collections.Generic;

namespace Client.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService (HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CreateOrderRequestDto> CreateOrderAsync(CreateOrderRequestDto order)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7140/gateway/orders", order);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<CreateOrderRequestDto>();
        }
        public async Task<List<OrderDto>> GetOrderByUserAsync(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7140/gateway/orders/user/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<OrderDto>>();
        }
        public async Task<List<ProductDto>> GetProductsByIdsAsync(List<int> productIds)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7140/gateway/products/batch", productIds);
            return await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        }
    }
}

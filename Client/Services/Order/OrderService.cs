using DTOs.Admin;
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
        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7140/gateway/orders/{id}");
            return await response.Content.ReadFromJsonAsync<OrderDto>();
        }

        public async Task<List<FullOrderDto>> GetAllOrdersAsync()
        {
            var response = await _httpClient.GetAsync($"https://localhost:7140/gateway/orders/all");
            return await response.Content.ReadFromJsonAsync<List<FullOrderDto>>();
        }
    }
}

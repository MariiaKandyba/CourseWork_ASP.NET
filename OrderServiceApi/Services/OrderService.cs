using DTOs.Orders;
using DTOs.Products;
using Microsoft.EntityFrameworkCore;
using OrderServiceApi.Data;
using OrderServiceApi.Models;
using System.Net.Http;

namespace OrderServiceApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient; 
        private readonly OrderDbContext _context;

        public OrderService(OrderDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public async Task<OrderDto> CreateOrder(int userId, List<OrderItemDto> items, AddressDto deliveryAddress)
        {
            var order = new Order
            {
                IdUser = userId,
                OrderItems = items.Select(item => new OrderItem
                {
                    IdProduct = item.ProductId,
                    Quantity = item.Quantity,
                }).ToList(),
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                Address = new Address()
                {
                    Country = deliveryAddress.Country,
                    City = deliveryAddress.City,
                    Street = deliveryAddress.Street,
                    ZipCode = deliveryAddress.ZipCode
                }
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return MapToDto(order);
        }

        public async Task<OrderDto> GetOrderByIdAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.Address) // Додаємо включення адреси
                .FirstOrDefaultAsync(o => o.Id == orderId);

            return MapToDto(order);
        }


        public async Task<List<OrderDto>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _context.Orders
                .Where(o => o.IdUser == userId)
                .Include(o => o.OrderItems)
                .Include(o => o.Address)
                .ToListAsync();

            return orders.Select(MapToDto).ToList();
        }

        private OrderDto MapToDto(Order order)
        {
            if (order == null) return null;

            return new OrderDto
            {
                Id = order.Id,
                UserId = order.IdUser,
                Items = order.OrderItems.Select(item => new OrderItemDto
                {
                    Id = item.Id,
                    ProductId = item.IdProduct,
                    Quantity = item.Quantity,
                }).ToList(),
                CreatedAt = order.CreatedAt,
                Status = order.Status.ToString(),
                DeliveryAddress = new AddressDto
                {
                    Street = order.Address.Street,
                    City = order.Address.City,
                    ZipCode = order.Address.ZipCode,
                    Country = order.Address.Country
                }
            };
        }

        public async Task<List<ProductDto>> GetProductsByIdsAsync(List<int> productIds)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7140/gateway/products/batch", productIds);
            return await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        }
    }

}

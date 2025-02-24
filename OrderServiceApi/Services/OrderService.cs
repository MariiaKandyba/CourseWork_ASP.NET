using DTOs.Orders;
using Microsoft.EntityFrameworkCore;
using OrderServiceApi.Models;
using OrderServiceApi.Repositories;

namespace OrderServiceApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
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
                Address = new Address
                {
                    Country = deliveryAddress.Country,
                    City = deliveryAddress.City,
                    Street = deliveryAddress.Street,
                    ZipCode = deliveryAddress.ZipCode
                }
            };

            order = await _orderRepository.AddOrderAsync(order);
            return MapToDto(order);
        }

        public async Task<OrderDto> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            return MapToDto(order);
        }

        public async Task<List<OrderDto>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            return orders.Select(MapToDto).ToList();
        }

        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            return orders.Select(MapToDto).ToList();
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            if (Enum.TryParse<OrderStatus>(newStatus, out var status))
            {
                return await _orderRepository.UpdateOrderStatusAsync(orderId, status);
            }
            return false;
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
    }
}

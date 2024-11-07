using DTOs.Orders;
using OrderServiceApi.Models;

namespace OrderServiceApi.Services
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrder(int userId, List<OrderItemDto> items, AddressDto deliveryAddress);
        Task<OrderDto> GetOrderByIdAsync(int orderId);
        Task<List<OrderDto>> GetOrdersByUserIdAsync(int userId);
        Task<List<OrderDto>> GetAllOrdersAsync();

    }

}

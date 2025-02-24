using OrderServiceApi.Models;

namespace OrderServiceApi.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<List<Order>> GetOrdersByUserIdAsync(int userId);
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order> AddOrderAsync(Order order);
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus);
    }
}

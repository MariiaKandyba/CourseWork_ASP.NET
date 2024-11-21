using DTOs.Admin;
using DTOs.Orders;

namespace Client.Services.Order
{
    public interface IOrderService
    {
        Task<CreateOrderRequestDto> CreateOrderAsync(CreateOrderRequestDto order, string token);
        Task<List<OrderDto>> GetOrderByUserAsync(int id, string token);
        Task<OrderDto> GetOrderByIdAsync(int id, string token);
        Task<List<FullOrderDto>> GetAllOrdersAsync(string token);
        Task<bool> UpdateOrderStatusAsync(int orderId, string newStatus, string token);

    }
}

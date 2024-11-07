using DTOs.Admin;
using DTOs.Orders;

namespace Client.Services.Order
{
    public interface IOrderService
    {
        Task<CreateOrderRequestDto> CreateOrderAsync(CreateOrderRequestDto order);
        Task<List<OrderDto>> GetOrderByUserAsync(int id);
        Task<OrderDto> GetOrderByIdAsync(int id);
        Task<List<FullOrderDto>> GetAllOrdersAsync();

    }
}

using DTOs.Orders;

namespace Client.Services.Order
{
    public interface IOrderService
    {
        Task<CreateOrderRequestDto> CreateOrderAsync(CreateOrderRequestDto order);
        Task<List<OrderDto>> GetOrderByUserAsync(int id);

    }
}

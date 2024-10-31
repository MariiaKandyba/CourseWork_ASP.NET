using DTOs.Orders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderServiceApi.Services;

namespace OrderServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequestDto request)
        {
            if (request == null || request.Items == null || request.Items.Count == 0)
            {
                return BadRequest("Order items are required.");
            }

            var orderDto = await _orderService.CreateOrder(request.UserId, request.Items, request.DeliveryAddress);
            return CreatedAtAction(nameof(GetOrderById), new { orderId = orderDto.Id }, orderDto);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var orderDto = await _orderService.GetOrderByIdAsync(orderId);
            if (orderDto == null)
            {
                return NotFound();
            }
            return Ok(orderDto);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUserId(int userId)
        {
            var ordersDto = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(ordersDto);
        }
    }
}

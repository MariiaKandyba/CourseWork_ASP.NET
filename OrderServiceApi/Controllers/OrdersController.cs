using DTOs.Admin;
using DTOs.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderServiceApi.Services;

namespace OrderServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ProductService _productService;
        private readonly UserService _userService;

        public OrdersController(IOrderService orderService, ProductService productService, UserService userService)
        {
            _orderService = orderService;
            _productService = productService;
            _userService = userService;
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
        [HttpGet("all")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var ordersDto = await _orderService.GetAllOrdersAsync();  
            var productsDto = await _productService.GetProductsAsync(token);  
            var usersDto = await _userService.GetAllUsersAsync(token);  

            var fullOrders = ordersDto.Select(order => new FullOrderDto
            {
                Id = order.Id,
                CreatedAt = order.CreatedAt,
                Status = order.Status,
                TotalPrice = order.Items.Sum(item =>
                    productsDto.FirstOrDefault(product => product.Id == item.ProductId)?.Price * item.Quantity ?? 0),  
                User = usersDto.FirstOrDefault(user => user.Id == order.UserId), 
                DeliveryAddress = order.DeliveryAddress, 
                Items = order.Items.Select(item => new FullOrderItemDto
                {
                    Item = productsDto.FirstOrDefault(product => product.Id == item.ProductId),  
                    Quantity = item.Quantity,
                    Price = productsDto.FirstOrDefault(product => product.Id == item.ProductId)?.Price ?? 0  
                }).ToList()
            }).ToList();

            return Ok(fullOrders);
        }

        [HttpPost("update-status")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.NewStatus))
            {
                return BadRequest("Invalid status.");
            }

            var result = await _orderService.UpdateOrderStatusAsync(request.OrderId, request.NewStatus);

            if (result)
            {
                return Ok("Order status updated successfully.");
            }

            return NotFound("Order not found.");
        }




    }
}

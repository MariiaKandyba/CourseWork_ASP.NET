using DTOs.Admin;
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
        public async Task<IActionResult> GetAllOrders()
        {
            var ordersDto = await _orderService.GetAllOrdersAsync();  // Отримання всіх замовлень
            var productsDto = await _productService.GetProductsAsync();  // Отримання всіх продуктів
            var usersDto = await _userService.GetAllUsersAsync();  // Отримання всіх користувачів

            var fullOrders = ordersDto.Select(order => new FullOrderDto
            {
                Id = order.Id,
                CreatedAt = order.CreatedAt,
                Status = order.Status,
                TotalPrice = order.Items.Sum(item =>
                    productsDto.FirstOrDefault(product => product.Id == item.ProductId)?.Price * item.Quantity ?? 0),  // Розрахунок загальної ціни з прайсів продуктів
                User = usersDto.FirstOrDefault(user => user.Id == order.UserId),  // Пошук користувача за id
                DeliveryAddress = order.DeliveryAddress,  // Адреса доставки
                Items = order.Items.Select(item => new FullOrderItemDto
                {
                    Item = productsDto.FirstOrDefault(product => product.Id == item.ProductId),  // Пошук продукту за id
                    Quantity = item.Quantity,
                    Price = productsDto.FirstOrDefault(product => product.Id == item.ProductId)?.Price ?? 0  // Витягування ціни продукту
                }).ToList()
            }).ToList();

            return Ok(fullOrders);
        }

    }
}

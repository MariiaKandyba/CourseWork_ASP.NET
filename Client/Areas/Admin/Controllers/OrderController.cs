using Client.Models;
using Client.Services.Order;
using Client.Services.Products;
using DTOs.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Request.Cookies["jwtToken"];
            var orders = await _orderService.GetAllOrdersAsync(token);
            return View(orders);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.NewStatus))
            {
                return BadRequest("Invalid data.");
            }
            var token = HttpContext.Request.Cookies["jwtToken"];

            var result = await _orderService.UpdateOrderStatusAsync(request.OrderId, request.NewStatus, token);

            if (result)
            {
                return Json(new { message = "Order status updated successfully." });
            }

            return Json(new { message = "Order not found." });
        }

    }
}

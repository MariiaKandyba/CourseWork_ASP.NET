using Client.Extensions;
using Client.Models;
using Client.Services.Order;
using Client.Services.Products;
using DTOs.Orders;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using UserServiceApi.Models;

namespace Client.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var addressDto = new AddressDto
            {
                Street = "123 Main St",
                City = "Sample City",
                ZipCode = "12345",
                Country = "Sample Country"
            };

            return View(addressDto);
        }
        [HttpPost]
        public IActionResult SubmitOrder(AddressDto address)
        {
            CreateOrderRequestDto create = new();

            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var em = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var userNameg = User.FindFirst("firstName")?.Value;
            var userSur= User.FindFirst("lastName")?.Value;
            int userId = int.TryParse(userIdString, out int id) ? id : 0; 

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");

            var orderItems = cart.Select(item => new OrderItemDto
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity
            }).ToList();

            create.Items = orderItems; 
            create.DeliveryAddress = address;
            create.UserId = userId;

            var userDetails = new UserDetailsViewModel
            {
                FirstName = User.FindFirst("firstName")?.Value,
                LastName = User.FindFirst("lastName")?.Value,
                Email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value,
                Address = new AddressDto
                {
                    Street = address.Street,
                    City = address.City,
                    ZipCode = address.ZipCode,
                    Country = address.Country
                }
            };
            TempData["UserDetails"] = JsonConvert.SerializeObject(userDetails);
            return RedirectToAction("Index", "Cart");
           
        }

        [HttpPost]
        public IActionResult SubmitFinalOrder(OrderViewModel order)
        {
                CreateOrderRequestDto create = new();
            var o = order;
                var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                int userId = int.TryParse(userIdString, out int id) ? id : 0;

                var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
                var orderItems = cart.Select(item => new OrderItemDto
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                }).ToList();

                create.Items = orderItems;
                create.DeliveryAddress = order.DeliveryAddress;
                create.UserId = userId;

                var a = _orderService.CreateOrderAsync(create);
                return RedirectToAction("Orders");
               
        }
        [HttpGet]
        public async Task<IActionResult> Orders(OrderViewModel order)
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            int userId = int.TryParse(userIdString, out int id) ? id : 0;

            var orders = await _orderService.GetOrderByUserAsync(userId);

            if (orders == null || !orders.Any())
            {
                return Content("NoOrders"); 
            }

            return View(orders);
        }


    }
}

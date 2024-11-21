using Client.Extensions;
using Client.Models;
using Client.Services.Order;
using Client.Services.Products;
using DTOs.Orders;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using DTOs.Products;
using Microsoft.AspNetCore.Authorization;

namespace Client.Controllers
{
    [Authorize]

    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ProductService _productService;

        public OrderController(OrderService orderService, ProductService productService)
        {
            _orderService = orderService;
            _productService = productService;
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
                var token = HttpContext.Request.Cookies["jwtToken"];
                var a = _orderService.CreateOrderAsync(create, token);
                HttpContext.Session.Remove("Cart");
                return RedirectToAction("Orders");
               
        }
        [HttpGet]
        public async Task<IActionResult> Orders(OrderViewModel order)
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            int userId = int.TryParse(userIdString, out int id) ? id : 0;
            var token = HttpContext.Request.Cookies["jwtToken"];
            var orders = await _orderService.GetOrderByUserAsync(userId, token);

            if (orders == null || orders.Count == 0)
            {
                return RedirectToAction("Index", "Products");
            }

            return View(orders);
        }
        [HttpGet]
        public async Task<IActionResult> OrderDetails(int id)
        {
            var token = HttpContext.Request.Cookies["jwtToken"];
            var orderDetails = await _orderService.GetOrderByIdAsync(id, token);

            if (orderDetails == null)
            {
                return NotFound("Order not found.");
            }
            var ids = orderDetails.Items.Select(item => item.ProductId).ToList();

            var products = await _productService.GetProductsByIdsAsync(ids);

            var orderViewModel = new OrderViewModel
            {
                DeliveryAddress = orderDetails.DeliveryAddress,
                Status = orderDetails.Status,
                TotalPrice = orderDetails.Items.Sum(item => item.Quantity * products.First(p => p.Id == item.ProductId).Price),
                Items = orderDetails.Items.Select(item => new ProductCartViewModel
                {
                    Id = item.ProductId,
                    Name = products.First(p => p.Id == item.ProductId).Name,
                    Price = products.First(p => p.Id == item.ProductId).Price,
                    ImageUrl = products.First(p => p.Id == item.ProductId).Images[0].ImageUrl,
                    Description = products.First(p => p.Id == item.ProductId).Description,
                    Quantity = item.Quantity
                }).ToList()
            };

            return View(orderViewModel);
        }



    }
}

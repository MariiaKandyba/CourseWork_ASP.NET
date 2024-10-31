using Client.Services.Products;
using DTOs.Products;
using Microsoft.AspNetCore.Mvc;
using Client.Extensions;
using Client.Models;
using Newtonsoft.Json;


namespace Client.Controllers
{
    public class CartController : Controller
    {
        private readonly ProductService _productService;
        private readonly CurrentUser _currentUser;


        public CartController(ProductService productService, CurrentUser currentUser)
        {
            _productService = productService;
            _currentUser = currentUser;
        }

        //public async Task<IActionResult>Index()
        //{
        //    var cart = GetCartFromSession();
        //    var productIds = cart.Select(item => item.ProductId).ToList();

        //    var products = await _productService.GetProductsByIdsAsync(productIds);

        //    var productCartViewModels = products.Select(product => new ProductCartViewModel
        //    {
        //        Id = product.Id,
        //        Name = product.Name,
        //        Price = product.Price,
        //        ImageUrl = product.Images.FirstOrDefault()?.ImageUrl,
        //        Description = product.Description,
        //        Quantity = cart.FirstOrDefault(item => item.ProductId == product.Id)?.Quantity ?? 0 
        //    }).ToList();
        //    decimal totalAmount = productCartViewModels.Sum(item => item.Price * item.Quantity);


        //    return View(productCartViewModels);
        //}

        public async Task<IActionResult> Index()
        {
            var cart = GetCartFromSession();
            var productIds = cart.Select(item => item.ProductId).ToList();
            var products = await _productService.GetProductsByIdsAsync(productIds);

            var productCartViewModels = products.Select(product => new ProductCartViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.Images.FirstOrDefault()?.ImageUrl,
                Description = product.Description,
                Quantity = cart.FirstOrDefault(item => item.ProductId == product.Id)?.Quantity ?? 0
            }).ToList();

            decimal totalAmount = productCartViewModels.Sum(item => item.Price * item.Quantity);

            var userDetailsJson = TempData["UserDetails"] as string;
            UserDetailsViewModel userDetails = null;

            if (!string.IsNullOrEmpty(userDetailsJson))
            {
                userDetails = JsonConvert.DeserializeObject<UserDetailsViewModel>(userDetailsJson);
            }

            var model = new OrderViewModel
            {
                Items = productCartViewModels,
                TotalPrice = totalAmount,
                FirstName = userDetails?.FirstName,
                LastName = userDetails?.LastName,
                Email = userDetails?.Email,
                DeliveryAddress = userDetails?.Address
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult GetCartItems()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            return Json(cart);
        }

        [HttpPost]
        public IActionResult Add(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var existingItem = cart.FirstOrDefault(c => c.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.Add(new CartItem { ProductId = productId, Quantity = 1 });
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);

            return Json(new { cartItemCount = cart.Sum(item => item.Quantity) });
        }
        [HttpGet]
        public JsonResult GetCartItemCount()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            int itemCount = cart.Count; 
            return Json(new { count = itemCount });
        }
        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCartFromSession();
            var product = cart.FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                cart.Remove(product);
                SaveCartToSession(cart);
            }

            return RedirectToAction("Index");
        }

        public IActionResult ClearCart()
        {
            SaveCartToSession(new List<CartItem>());
            return RedirectToAction("Index");
        }
        private List<CartItem> GetCartFromSession()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
            return cart ?? new List<CartItem>();
        }

        private void SaveCartToSession(List<CartItem> cart)
        {
            HttpContext.Session.SetObjectAsJson("Cart", cart);
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int change)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var cartItem = cart.FirstOrDefault(item => item.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity += change;

                if (cartItem.Quantity < 1)
                {
                    cartItem.Quantity = 1;
                }
                
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return Json(new { newQuantity = cartItem?.Quantity ?? 0 });
        }
        [HttpPost]
        public IActionResult Remove(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var cartItem = cart.FirstOrDefault(item => item.ProductId == productId);
            if (cartItem != null)
            {
                cart.Remove(cartItem);
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);

            return Json(new { success = true });
        }


    }
}

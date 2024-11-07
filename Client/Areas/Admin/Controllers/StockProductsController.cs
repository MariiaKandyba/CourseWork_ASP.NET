using Client.Areas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class StockProductsController : Controller
    {
        private readonly StockProductService _productService;

        public StockProductsController(StockProductService userService)
        {
            _productService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var a = await _productService.GetAll();
            return View(a);
        }
    }
}

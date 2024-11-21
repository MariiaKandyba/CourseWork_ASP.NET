using Client.Services.Products;
using DTOs.Products;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace Client.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductService _productService;
        private const int PageSize = 3;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index(int page = 1, List<string> categories = null)
        {
            List<CategoryDto> categoryDtos = categories?.Select(name => new CategoryDto { Name = name }).ToList();

            var paginatedProducts = await _productService.GetPaginatedProductsAsync(page, PageSize, categoryDtos);
            int totalProducts = await _productService.GetTotalProductCountAsync(categoryDtos);
            int totalPages = (int)Math.Ceiling((double)totalProducts / PageSize);
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SelectedCategories = categories;

            return View(paginatedProducts);
        }
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> AddReview(int productId, int rating, string comment)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 


            var reviewDto = new ReviewDto
            {
                Rating = rating,
                Comment = comment,
                ProductId = productId,
                UserId = int.Parse(userId)
            };

            var success = await _productService.AddReviewAsync(reviewDto);

            if (success)
            {
                return RedirectToAction("Details", new { id = productId });
            }

            TempData["Error"] = "Failed to add review.";
            return RedirectToAction("Details", new { id = productId });
        }


    }
}

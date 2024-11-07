using Client.Services.Products;
using DTOs.Products;
using Microsoft.AspNetCore.Mvc;

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

    }
}

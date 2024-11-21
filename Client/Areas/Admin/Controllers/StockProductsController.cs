using Client.Areas.Services;
using DTOs.Admin;
using DTOs.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Http;

namespace Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class StockProductsController : Controller
    {
        private readonly StockProductService _productService;

        public StockProductsController(StockProductService productService)
        {
            _productService = productService;
        }

        // GET: Admin/StockProducts
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Request.Cookies["jwtToken"];
            var products = await _productService.GetAll(token);
            return View(products);
        }

        // GET: Admin/StockProducts/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var token = HttpContext.Request.Cookies["jwtToken"];

            var product = await _productService.GetById(id, token);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: Admin/StockProducts/Create
        public async Task<IActionResult> Create()
        {
            var token = HttpContext.Request.Cookies["jwtToken"];

            var listCat = await _productService.GetCategories();
            ViewBag.Categories = listCat.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
            var listBrand= await _productService.GetBrands();
            ViewBag.Brands = listBrand.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
            return View();
        }

        // POST: Admin/StockProducts/Create
       [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductCreateDto productDto)
    {
            var token = HttpContext.Request.Cookies["jwtToken"];

            if (ModelState.IsValid)
        {
            await _productService.Create(productDto, token);
            return RedirectToAction(nameof(Index));
        }

        return View(productDto);
    }



        // GET: Admin/StockProducts/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var token = HttpContext.Request.Cookies["jwtToken"];

            var listCat = await _productService.GetCategories();
            ViewBag.Categories = listCat.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            var listBrand = await _productService.GetBrands();
            ViewBag.Brands = listBrand.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            var product = await _productService.GetById(id, token);
            if (product == null)
            {
                return NotFound();
            }

            var newP = new ProductEditDto()
            {
                Id = product.Product.Id,
                Name = product.Product.Name,
                IsAvailable = product.Product.IsAvailable,
                Price = product.Product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.Product.Category.Id, 
                BrandId = product.Product.Brand.Id 
            };

            return View(newP);
        }


        // POST: Admin/StockProducts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductEditDto productDto)
        {
            var token = HttpContext.Request.Cookies["jwtToken"];


            if (ModelState.IsValid)
            {
                await _productService.Update(productDto, token);
                return RedirectToAction(nameof(Index));
            }
            return View(productDto);
        }

        // GET: Admin/StockProducts/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.Request.Cookies["jwtToken"];

            var product = await _productService.GetById(id, token);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost, ActionName("Delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteProduct(int id)
        {
            var token = HttpContext.Request.Cookies["jwtToken"];

            await _productService.Delete(id, token);
                return RedirectToAction("Index", "StockProducts"); 
        }





    }
}

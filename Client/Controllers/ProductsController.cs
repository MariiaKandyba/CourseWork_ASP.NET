﻿using Client.Services.Products;
using DTOs.Products;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDto> products = await _productService.GetProductsAsync();
            return View(products);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTOs.Admin;
using DTOs.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductServiceApi.Data;
using ProductServiceApi.Models;
using ProductServiceApi.Services;

namespace ProductServiceApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        
        [HttpPost("batch")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByIds([FromBody] List<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return BadRequest("Cannot be empty");
            }

            var productDtos = await _productService.GetProductsByIdsAsync(ids);
            return Ok(productDtos);
        }

        [HttpPost("reviews")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> PostReview([FromBody] ReviewDto review)
        {
            if (review == null)
            {
                return BadRequest("Cannot be empty");
            }

            var rev = await _productService.CreateReviewAsync(review);
            return Ok(rev);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var productDtos = await _productService.GetAllProductsAsync();
            return Ok(productDtos);
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("full")]
        public async Task<ActionResult<IEnumerable<FullProductDto>>> GetFullProducts()
        {
            var productDtos = await _productService.GetFullProductsAsync();
            return Ok(productDtos);
        }




        [HttpGet("full/{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<FullProductDto>> GetFullProductById(int id)
        {
            var productDto = await _productService.GetFullProductByIdAsync(id);
            if (productDto == null)
            {
                return NotFound();
            }
            return Ok(productDto);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var productDto = await _productService.GetProductByIdAsync(id);
            if (productDto == null)
            {
                return NotFound();
            }
            return Ok(productDto);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> PutProduct(int id, ProductEditDto productEditDto)
        {
            if (productEditDto == null)
            {
                return BadRequest("Invalid product data.");
            }

            if (id != productEditDto.Id)
            {
                return BadRequest("Product ID mismatch.");
            }

            var result = await _productService.UpdateProductAsync(id, productEditDto);

            if (!result)
            {
                return NotFound("Product not found.");
            }

            return NoContent();
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<ProductDto>> PostProduct(ProductCreateDto fullProductDto)
        {
            if (fullProductDto == null)
            {
                return BadRequest("Invalid product data.");
            }

            var createdProductDto = await _productService.CreateProductAsync(fullProductDto);

            if (createdProductDto == null)
            {
                return BadRequest("Category or Brand not found.");
            }

            return CreatedAtAction("GetProduct", new { id = createdProductDto.Id }, createdProductDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteProduct(int id)
        {
            var isDeleted = await _productService.DeleteProductAsync(id);

            if (!isDeleted)
            {
                return NotFound("Product not found");
            }

            return NoContent();
        }



        [HttpGet("count")]
        public async Task<int> GetTotalProductCount([FromQuery] string categories = null)
        {
            return await _productService.GetTotalProductCountAsync(categories);
        }

        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetPaginatedProducts([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] List<string> categories = null)
        {
            var productDtos = await _productService.GetPaginatedProducts(page, pageSize, categories);
            return Ok(productDtos);

        }
    }
   
}


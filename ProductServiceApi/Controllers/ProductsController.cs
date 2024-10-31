using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTOs.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductServiceApi.Data;
using ProductServiceApi.Models;

namespace ProductServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductDbContext _context;

        public ProductsController(ProductDbContext context)
        {
            _context = context;
        }
        [HttpPost("batch")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByIds([FromBody] List<int> ids)
        {
            var products = await _context.Products
                .Where(p => ids.Contains(p.Id))
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Specifications)
                .Include(p => p.Images)
                .Include(p => p.Reviews)
                .ToListAsync();

            var productDtos = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                IsAvailable = p.IsAvailable,
                Category = new CategoryDto
                {
                    Id = p.Category.Id,
                    Name = p.Category.Name
                },
                Brand = new BrandDto
                {
                    Id = p.Brand.Id,
                    Name = p.Brand.Name
                },
                Specifications = p.Specifications.Select(s => new SpecificationDto
                {
                    Key = s.Key,
                    Value = s.Value
                }).ToList(),
                Images = p.Images.Select(i => new ImageDto
                {
                    ImageUrl = i.ImageUrl
                }).ToList(),
                Reviews = p.Reviews.Select(r => new ReviewDto
                {
                    Rating = r.Rating,
                    Comment = r.Comment,
                    ReviewDate = r.ReviewDate
                }).ToList()
            }).ToList();

            return Ok(productDtos);
        }


        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Specifications)
                .Include(p => p.Images)
                .Include(p => p.Reviews)
                .ToListAsync();

            var productDtos = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                IsAvailable = p.IsAvailable,
                Category = new CategoryDto
                {
                    Id = p.Category.Id,
                    Name = p.Category.Name
                },
                Brand = new BrandDto
                {
                    Id = p.Brand.Id,
                    Name = p.Brand.Name
                },
                Specifications = p.Specifications.Select(s => new SpecificationDto
                {
                    Key = s.Key,
                    Value = s.Value
                }).ToList(),
                Images = p.Images.Select(i => new ImageDto
                {
                    ImageUrl = i.ImageUrl
                }).ToList(),
                Reviews = p.Reviews.Select(r => new ReviewDto
                {
                    Rating = r.Rating,
                    Comment = r.Comment,
                    ReviewDate = r.ReviewDate
                }).ToList()
            }).ToList(); 

            return Ok(productDtos); 
        }


        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Specifications)
                .Include(p => p.Images)
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                IsAvailable = product.IsAvailable,
                Category = new CategoryDto
                {
                    Id = product.Category.Id,
                    Name = product.Category.Name
                },
                Brand = new BrandDto
                {
                    Id = product.Brand.Id,
                    Name = product.Brand.Name
                },
                Specifications = product.Specifications.Select(s => new SpecificationDto
                {
                    Key = s.Key,
                    Value = s.Value
                }).ToList(),
                Images = product.Images.Select(i => new ImageDto
                {
                    ImageUrl = i.ImageUrl
                }).ToList(),
                Reviews = product.Reviews.Select(r => new ReviewDto
                {
                    Rating = r.Rating,
                    Comment = r.Comment,
                    ReviewDate = r.ReviewDate
                }).ToList()
            };

            return Ok(productDto);
        }


        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}

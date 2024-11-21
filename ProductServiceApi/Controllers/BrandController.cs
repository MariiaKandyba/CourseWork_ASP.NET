using DTOs.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductServiceApi.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly ProductDbContext _context;

        public BrandController(ProductDbContext context)
        {
            _context = context;
        }
        [HttpGet]

        public async Task<ActionResult<List<BrandDto>>> GetBrands()
        {
            var brands = await _context.Brands
                .Select(c => new BrandDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();

            return Ok(brands);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BrandDto>> GetBrandById(int id)
        {
            var brands = await _context.Categories
                .Where(c => c.Id == id)
                .Select(c => new BrandDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .FirstOrDefaultAsync();

            if (brands == null)
            {
                return NotFound($"brands with ID {id} not found.");
            }

            return Ok(brands);
        }
    }
}

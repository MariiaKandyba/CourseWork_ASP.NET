using DTOs.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductServiceApi.Data;
using ProductServiceApi.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<ActionResult<List<BrandDto>>> GetBrands()
        {
            var brands = await _brandService.GetBrandsAsync();
            return Ok(brands);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BrandDto>> GetBrandById(int id)
        {
            var brand = await _brandService.GetBrandByIdAsync(id);
            if (brand == null)
            {
                return NotFound($"Brand with ID {id} not found.");
            }

            return Ok(brand);
        }
    }
}

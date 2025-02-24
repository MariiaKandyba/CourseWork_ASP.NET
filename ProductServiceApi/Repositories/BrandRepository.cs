using Microsoft.EntityFrameworkCore;
using ProductServiceApi.Data;
using ProductServiceApi.Models;

namespace ProductServiceApi.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly ProductDbContext _context;

        public BrandRepository(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<List<Brand>> GetAllBrandsAsync()
        {
            return await _context.Brands.ToListAsync();
        }

        public async Task<Brand> GetBrandByIdAsync(int id)
        {
            return await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}

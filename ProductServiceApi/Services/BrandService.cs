using DTOs.Products;
using ProductServiceApi.Repositories;

namespace ProductServiceApi.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;

        public BrandService(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<List<BrandDto>> GetBrandsAsync()
        {
            var brands = await _brandRepository.GetAllBrandsAsync();
            return brands.Select(b => new BrandDto
            {
                Id = b.Id,
                Name = b.Name
            }).ToList();
        }

        public async Task<BrandDto> GetBrandByIdAsync(int id)
        {
            var brand = await _brandRepository.GetBrandByIdAsync(id);
            if (brand == null) return null;

            return new BrandDto
            {
                Id = brand.Id,
                Name = brand.Name
            };
        }
    }
}

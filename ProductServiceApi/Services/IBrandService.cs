using DTOs.Products;

namespace ProductServiceApi.Services
{
    public interface IBrandService
    {
        Task<List<BrandDto>> GetBrandsAsync();
        Task<BrandDto> GetBrandByIdAsync(int id);
    }
}

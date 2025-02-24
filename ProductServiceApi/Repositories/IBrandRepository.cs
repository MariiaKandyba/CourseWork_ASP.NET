using ProductServiceApi.Models;

namespace ProductServiceApi.Repositories
{
    public interface IBrandRepository
    {
        Task<List<Brand>> GetAllBrandsAsync();
        Task<Brand> GetBrandByIdAsync(int id);
    }
}

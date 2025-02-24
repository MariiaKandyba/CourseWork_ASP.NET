using ProductServiceApi.Models;

namespace ProductServiceApi.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int id);
    }
}

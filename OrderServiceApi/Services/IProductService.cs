using DTOs.Products;

namespace OrderServiceApi.Services
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetProductsAsync(string token);
        Task<ProductDto> GetProductByIdAsync(int id);
        Task<List<ProductDto>> GetProductsByIdsAsync(List<int> productIds);
        Task<List<ProductDto>> GetPaginatedProductsAsync(int page, int pageSize, List<CategoryDto> categories);
        Task<int> GetTotalProductCountAsync(List<CategoryDto> categories);
    }
}

using DTOs.Admin;
using DTOs.Products;

namespace ProductServiceApi.Services
{
    public interface IProductService
    {
        Task<ProductDto> GetProductByIdAsync(int id);
        Task<ProductDto?> CreateProductAsync(ProductCreateDto productCreateDto);
        Task<bool> UpdateProductAsync(int id, ProductEditDto productEditDto);
        Task<bool> DeleteProductAsync(int productId);
        Task<IEnumerable<ProductDto>> GetProductsByIdsAsync(List<int> ids);
        Task<FullProductDto> GetFullProductByIdAsync(int id);
        Task<IEnumerable<FullProductDto>> GetFullProductsAsync();
        Task<int> GetTotalProductCountAsync(string categories = null);
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();

        Task<IEnumerable<ProductDto>> GetPaginatedProducts(int page = 1, int pageSize = 10, List<string> categories = null);
         Task<bool> CreateReviewAsync(ReviewDto review);

    }


}

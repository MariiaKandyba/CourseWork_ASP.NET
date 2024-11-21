using DTOs.Admin;
using DTOs.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductServiceApi.Data;
using ProductServiceApi.Models;

namespace ProductServiceApi.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductDbContext _context;

        public ProductService(ProductDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<FullProductDto>> GetFullProductsAsync()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Specifications)
                .Include(p => p.Images)
                .Include(p => p.Reviews)
                .ToListAsync();

            var productDtos = products.Select(p => new FullProductDto
            {
                Product = new ProductDto
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
                },
                StockQuantity = p.Stock
            }).ToList();

            return productDtos;
        }
        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Specifications)
                .Include(p => p.Images)
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                IsAvailable = product.IsAvailable,
                Category = product.Category != null ? new CategoryDto
                {
                    Id = product.Category.Id,
                    Name = product.Category.Name
                } : null,
                Brand = product.Brand != null ? new BrandDto
                {
                    Id = product.Brand.Id,
                    Name = product.Brand.Name
                } : null,
                Specifications = product.Specifications?.Select(s => new SpecificationDto
                {
                    Key = s.Key,
                    Value = s.Value
                }).ToList(),
                Images = product.Images?.Select(i => new ImageDto
                {
                    ImageUrl = i.ImageUrl
                }).ToList(),
                Reviews = product.Reviews?.Select(r => new ReviewDto
                {
                    Rating = r.Rating,
                    Comment = r.Comment,
                    ReviewDate = r.ReviewDate
                }).ToList()
            };
        }
        public async Task<bool> CreateReviewAsync(ReviewDto review)
        {
            var rev = new ProductReview()
            {
                UserId = (int)review.UserId,
                Comment = review.Comment,
                ProductId = review.ProductId,
                Rating = review.Rating,
            };
            _context.ProductReviews.Add(rev);
            await _context.SaveChangesAsync();
            return true;


        }

        public async Task<ProductDto?> CreateProductAsync(ProductCreateDto productCreateDto)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == productCreateDto.CategoryId);
            var brand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == productCreateDto.BrandId);

            if (category == null || brand == null)
            {
                return null; 
            }

            var product = new Product
            {
                Name = productCreateDto.Name,
                Description = productCreateDto.Description,
                Price = productCreateDto.Price,
                IsAvailable = productCreateDto.IsAvailable,
                Category = category,
                Brand = brand,
                Images = productCreateDto.Images?.Select(i => new ProductImage
                {
                    ImageUrl = i.ImageUrl
                }).ToList(),
                Stock = productCreateDto.StockQuantity
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                IsAvailable = product.IsAvailable,
                Category = new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name
                },
                Brand = new BrandDto
                {
                    Id = brand.Id,
                    Name = brand.Name
                },
                Specifications = product.Specifications?.Select(s => new SpecificationDto
                {
                    Key = s.Key,
                    Value = s.Value
                }).ToList(),
                Images = product.Images?.Select(i => new ImageDto
                {
                    ImageUrl = i.ImageUrl
                }).ToList(),
                Reviews = product.Reviews?.Select(r => new ReviewDto
                {
                    Rating = r.Rating,
                    Comment = r.Comment,
                    ReviewDate = r.ReviewDate
                }).ToList()
            };
        }


        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product == null)
            {
                return false; 
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true; 
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByIdsAsync(List<int> ids)
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

            return productDtos;
        }


        
        public async Task<FullProductDto?> GetFullProductByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Specifications)
                .Include(p => p.Images)
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return null;

            return new FullProductDto
            {
                Product = new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    IsAvailable = product.IsAvailable,
                    Category = product.Category != null ? new CategoryDto
                    {
                        Id = product.Category.Id,
                        Name = product.Category.Name
                    } : null,
                    Brand = product.Brand != null ? new BrandDto
                    {
                        Id = product.Brand.Id,
                        Name = product.Brand.Name
                    } : null,
                    Specifications = product.Specifications?.Select(s => new SpecificationDto
                    {
                        Key = s.Key,
                        Value = s.Value
                    }).ToList(),
                    Images = product.Images?.Select(i => new ImageDto
                    {
                        ImageUrl = i.ImageUrl
                    }).ToList(),
                    Reviews = product.Reviews?.Select(r => new ReviewDto
                    {
                        Rating = r.Rating,
                        Comment = r.Comment,
                        ReviewDate = r.ReviewDate
                    }).ToList()
                },
                StockQuantity = product.Stock
            };
        }



        public async Task<int> GetTotalProductCountAsync(string categories = null)
        {
            IQueryable<Product> query = _context.Products;

            if (!string.IsNullOrEmpty(categories))
            {
                string[] categoryNames = categories.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                query = query.Where(p => categoryNames.Contains(p.Category.Name));
            }

            return await query.CountAsync();
        }

        public async Task<IEnumerable<ProductDto>> GetPaginatedProducts(int page, int pageSize, List<string> categories = null)
        {
            int skip = (page - 1) * pageSize;

            IQueryable<Product> query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Specifications)
                .Include(p => p.Images)
                .Include(p => p.Reviews);

            if (categories != null && categories.Any())
            {
                var categoryNames = categories.FirstOrDefault().Split(',').Select(c => c.Trim()).ToList();
                query = query.Where(p => categoryNames.Contains(p.Category.Name));
            }

            var products = await query
                .Skip(skip)
                .Take(pageSize)
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

            return productDtos;
        }
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
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

            return productDtos;
        }

        

        public async Task<bool> UpdateProductAsync(int id, ProductEditDto productEditDto)
        {
            var existingProduct = await _context.Products
                .Include(c => c.Category)
                .Include(p => p.Brand)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (existingProduct == null)
            {
                return false;
            }

            existingProduct.Name = productEditDto.Name ?? existingProduct.Name;
            existingProduct.Price = productEditDto.Price != 0 ? productEditDto.Price : existingProduct.Price;
            existingProduct.IsAvailable = productEditDto.IsAvailable;
            existingProduct.Stock = productEditDto.StockQuantity != 0 ? productEditDto.StockQuantity : existingProduct.Stock;

            existingProduct.Category = await _context.Categories.FindAsync(productEditDto.CategoryId);
            existingProduct.Brand = await _context.Brands.FindAsync(productEditDto.BrandId);

            _context.Entry(existingProduct).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }
    }

}

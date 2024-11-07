using Microsoft.EntityFrameworkCore;
using ProductServiceApi.Models;
using System;

namespace ProductServiceApi.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) {
            Database.EnsureCreated();

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ProductSpecification> ProductSpecifications { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fluent API configurations
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId);

            modelBuilder.Entity<ProductSpecification>()
                .HasOne(ps => ps.Product)
                .WithMany(p => p.Specifications)
                .HasForeignKey(ps => ps.ProductId);

            modelBuilder.Entity<ProductImage>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(pi => pi.ProductId);

            modelBuilder.Entity<ProductReview>()
                .HasOne(pr => pr.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(pr => pr.ProductId);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Smartphones", Description = "Mobile devices" },
                new Category { Id = 2, Name = "Laptops", Description = "Portable computers" },
                new Category { Id = 3, Name = "Tablets", Description = "Handheld tablets" },
                new Category { Id = 4, Name = "Headphones", Description = "Audio devices" }
            );

            modelBuilder.Entity<Brand>().HasData(
                 new Brand { Id = 1, Name = "Apple", Description = "Apple Inc." },
                 new Brand { Id = 2, Name = "Samsung", Description = "Samsung Electronics" },
                 new Brand { Id = 3, Name = "Sony", Description = "Sony Corporation" },
                 new Brand { Id = 4, Name = "Dell", Description = "Dell Technologies" }
             );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "iPhone 14", Description = "Apple smartphone", Price = 999.99m, Stock = 50, CategoryId = 1, BrandId = 1, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow, IsAvailable = true },
                new Product { Id = 2, Name = "Galaxy S22", Description = "Samsung smartphone", Price = 899.99m, Stock = 75, CategoryId = 1, BrandId = 2, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow, IsAvailable = true },
                new Product { Id = 3, Name = "MacBook Pro", Description = "Apple laptop", Price = 1999.99m, Stock = 30, CategoryId = 2, BrandId = 1, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow, IsAvailable = true },
                new Product { Id = 4, Name = "XPS 13", Description = "Dell laptop", Price = 1499.99m, Stock = 20, CategoryId = 2, BrandId = 4, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow, IsAvailable = true },
                new Product { Id = 5, Name = "iPad Pro", Description = "Apple tablet", Price = 799.99m, Stock = 40, CategoryId = 3, BrandId = 1, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow, IsAvailable = true },
                new Product { Id = 6, Name = "Galaxy Tab S8", Description = "Samsung tablet", Price = 699.99m, Stock = 35, CategoryId = 3, BrandId = 2, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow, IsAvailable = true },
                new Product { Id = 7, Name = "WH-1000XM4", Description = "Sony headphones", Price = 349.99m, Stock = 60, CategoryId = 4, BrandId = 3, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow, IsAvailable = true },
                new Product { Id = 8, Name = "AirPods Pro", Description = "Apple wireless earbuds", Price = 249.99m, Stock = 100, CategoryId = 4, BrandId = 1, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow, IsAvailable = true }
            );

            modelBuilder.Entity<ProductSpecification>().HasData(
               new ProductSpecification { Id = 1, ProductId = 1, Key = "Screen Size", Value = "6.1 inches" },
               new ProductSpecification { Id = 2, ProductId = 1, Key = "Battery Life", Value = "Up to 18 hours" },
               new ProductSpecification { Id = 3, ProductId = 2, Key = "Screen Size", Value = "6.2 inches" },
               new ProductSpecification { Id = 4, ProductId = 2, Key = "Battery Life", Value = "Up to 20 hours" },
               new ProductSpecification { Id = 5, ProductId = 3, Key = "Processor", Value = "M1 Pro" },
               new ProductSpecification { Id = 6, ProductId = 3, Key = "RAM", Value = "16 GB" },
               new ProductSpecification { Id = 7, ProductId = 4, Key = "Processor", Value = "Intel i7" },
               new ProductSpecification { Id = 8, ProductId = 5, Key = "Screen Size", Value = "12.9 inches" },
               new ProductSpecification { Id = 9, ProductId = 6, Key = "Screen Size", Value = "11 inches" },
               new ProductSpecification { Id = 10, ProductId = 7, Key = "Noise Cancellation", Value = "Yes" },
               new ProductSpecification { Id = 11, ProductId = 8, Key = "Water Resistant", Value = "Yes" }
           );

            modelBuilder.Entity<ProductImage>().HasData(
                new ProductImage { Id = 1, ProductId = 1, ImageUrl = "https://content1.rozetka.com.ua/goods/images/big/175435404.jpg", IsPrimary = true },
                new ProductImage { Id = 2, ProductId = 2, ImageUrl = "https://images.samsung.com/is/image/samsung/p6pim/ua/sm-s908bzkgsek/gallery/ua-galaxy-s22-ultra-s908-sm-s908bzkgsek-531654748?$650_519_PNG$", IsPrimary = true },
                new ProductImage { Id = 3, ProductId = 3, ImageUrl = "https://content1.rozetka.com.ua/goods/images/big/144249716.jpg", IsPrimary = true },
                new ProductImage { Id = 4, ProductId = 4, ImageUrl = "https://content.rozetka.com.ua/goods/images/big/437114162.jpg", IsPrimary = true },
                new ProductImage { Id = 5, ProductId = 5, ImageUrl = "https://content.rozetka.com.ua/goods/images/big/301660400.jpg", IsPrimary = true },
                new ProductImage { Id = 6, ProductId = 6, ImageUrl = "https://content.rozetka.com.ua/goods/images/big/465008900.jpg", IsPrimary = true },
                new ProductImage { Id = 7, ProductId = 7, ImageUrl = "https://content2.rozetka.com.ua/goods/images/big/465046398.jpg", IsPrimary = true },
                new ProductImage { Id = 8, ProductId = 8, ImageUrl = "https://content.rozetka.com.ua/goods/images/big/459716439.jpg", IsPrimary = true }
            );

            modelBuilder.Entity<ProductReview>().HasData(
                new ProductReview { Id = 1, ProductId = 1, UserId = 1, Rating = 5, Comment = "Great phone!", ReviewDate = DateTime.UtcNow },
                new ProductReview { Id = 2, ProductId = 2, UserId = 2, Rating = 4, Comment = "Good value for money.", ReviewDate = DateTime.UtcNow }
            );
        }
    }
}

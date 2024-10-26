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

            // Seed data
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Smartphones", Description = "Mobile devices" },
                new Category { Id = 2, Name = "Laptops", Description = "Portable computers" }
            );

            modelBuilder.Entity<Brand>().HasData(
                new Brand { Id = 1, Name = "Apple", Description = "Apple Inc." },
                new Brand { Id = 2, Name = "Samsung", Description = "Samsung Electronics" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "iPhone 14",
                    Description = "Latest Apple smartphone",
                    Price = 999.99m,
                    Stock = 50,
                    CategoryId = 1,
                    BrandId = 1,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 2,
                    Name = "Galaxy S22",
                    Description = "Latest Samsung smartphone",
                    Price = 899.99m,
                    Stock = 75,
                    CategoryId = 1,
                    BrandId = 2,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                    IsAvailable = true
                }
            );

            modelBuilder.Entity<ProductSpecification>().HasData(
                new ProductSpecification { Id = 1, ProductId = 1, Key = "Screen Size", Value = "6.1 inches" },
                new ProductSpecification { Id = 2, ProductId = 1, Key = "Battery Life", Value = "Up to 18 hours" },
                new ProductSpecification { Id = 3, ProductId = 2, Key = "Screen Size", Value = "6.2 inches" },
                new ProductSpecification { Id = 4, ProductId = 2, Key = "Battery Life", Value = "Up to 20 hours" }
            );

            modelBuilder.Entity<ProductImage>().HasData(
                new ProductImage { Id = 1, ProductId = 1, ImageUrl = "https://content1.rozetka.com.ua/goods/images/big/175435404.jpg", IsPrimary = true },
                new ProductImage { Id = 2, ProductId = 2, ImageUrl = "https://images.samsung.com/is/image/samsung/p6pim/ua/sm-s908bzkgsek/gallery/ua-galaxy-s22-ultra-s908-sm-s908bzkgsek-531654748?$650_519_PNG$", IsPrimary = true }
            );

            modelBuilder.Entity<ProductReview>().HasData(
                new ProductReview { Id = 1, ProductId = 1, UserId = 1, Rating = 5, Comment = "Great phone!", ReviewDate = DateTime.UtcNow },
                new ProductReview { Id = 2, ProductId = 2, UserId = 2, Rating = 4, Comment = "Good value for money.", ReviewDate = DateTime.UtcNow }
            );
        }
    }
}

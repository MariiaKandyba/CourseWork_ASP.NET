using Microsoft.EntityFrameworkCore;
using OrderServiceApi.Models;

namespace OrderServiceApi.Data
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Address>().HasData(new Address
            {
                Id = 1,
                Street = "123 Main St",
                City = "Springfield",
                ZipCode = "62701",
                Country = "USA"
            });

            modelBuilder.Entity<Order>().HasData(
                new Order
                {
                    Id = 1,
                    IdUser = 2,
                    CreatedAt = DateTime.UtcNow,
                    Status = OrderStatus.Pending,
                    AddressId = 1 
                }
            );

            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem
                {
                    Id = 1,
                    OrderId = 1,
                    IdProduct = 1,
                    Quantity = 1,
                },
                new OrderItem
                {
                    Id = 2,
                    OrderId = 1,
                    IdProduct = 2,
                    Quantity = 2,
                }
            );
        }
    }
}

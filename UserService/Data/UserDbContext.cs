using Microsoft.EntityFrameworkCore;
using UserServiceApi.Models;

namespace UserServiceApi.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
            Database.EnsureCreated(); 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles) 
                .WithMany(r => r.Users) 
                .UsingEntity(j => j.ToTable("UserRoles")); 

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, FirstName = "Admin", LastName = "User", Email = "admin@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin")},
                new User { Id = 2, FirstName = "Regular", LastName = "User", Email = "user@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("123")}
            );

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "User" }
            );

            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity(j => j.HasData(
                    new { UsersId = 1, RolesId = 1 }, // Admin user has Admin role
                    new { UsersId = 2, RolesId = 2 }  // Regular user has User role
                ));
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}

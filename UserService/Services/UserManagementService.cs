using DTOs.Admin;
using DTOs.Auth;
using Microsoft.EntityFrameworkCore;
using UserServiceApi.Data;
using UserServiceApi.Models;

namespace UserServiceApi.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly UserDbContext _dbContext;

        public UserManagementService(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            return await _dbContext.Users
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Roles = u.Roles.Select(r => new RoleDto { Id = r.Id, Name = r.Name }).ToList()
                })
                .ToListAsync();
        }

        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            var user = await _dbContext.Users
                .Include(u => u.Roles)
                .Where(u => u.Id == userId)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Roles = u.Roles.Select(r => new RoleDto { Id = r.Id, Name = r.Name }).ToList()
                })
                .FirstOrDefaultAsync();

            if (user == null)
                throw new Exception($"User with ID {userId} not found.");

            return user;
        }

        public async Task<AuthResultDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            if (await _dbContext.Users.AnyAsync(u => u.Email == createUserDto.Email))
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "User already exists." };

            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == createUserDto.RoleId);
            if (role == null)
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "Role not found." };

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);

            var user = new User
            {
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                Email = createUserDto.Email,
                PasswordHash = hashedPassword,
                Roles = new List<Role> { role }
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return new AuthResultDto { IsSuccess = true };
        }

        public async Task<AuthResultDto> DeleteUserAsync(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "User not found." };

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            return new AuthResultDto { IsSuccess = true };
        }
    }
}

using DTOs.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using UserServiceApi.Data;
using UserServiceApi.Models;

namespace UserServiceApi.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserDbContext _dbContext;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthenticationService(UserDbContext dbContext, IJwtTokenService jwtTokenService)
        {
            _dbContext = dbContext;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<AuthResultDto> RegisterAsync(RegisterDto registerDto)
        {
            if (await _dbContext.Users.AnyAsync(u => u.Email == registerDto.Email))
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "User already exists." };

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var user = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                PasswordHash = hashedPassword
            };

            var defaultRole = await _dbContext.Roles.SingleOrDefaultAsync(r => r.Name == "User");
            if (defaultRole != null)
            {
                user.Roles = new List<Role> { defaultRole };
            }

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return new AuthResultDto { IsSuccess = true };
        }

        public async Task<AuthResultDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _dbContext.Users.Include(u => u.Roles).SingleOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "Invalid credentials." };

            var token = _jwtTokenService.GenerateJwtToken(user);
            return new AuthResultDto { IsSuccess = true, Token = token };
        }

        public async Task<AuthResultDto> UpdateProfileAsync(int userId, UpdateDto updateDto)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "User not found." };

            user.FirstName = updateDto.FirstName;
            user.LastName = updateDto.LastName;
            user.Email = updateDto.Email;

            await _dbContext.SaveChangesAsync();

            var token = _jwtTokenService.GenerateJwtToken(user);
            return new AuthResultDto { IsSuccess = true, Token = token };
        }
    }
}

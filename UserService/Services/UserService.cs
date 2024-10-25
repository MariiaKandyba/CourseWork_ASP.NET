using DTOs.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserServiceApi.Data;
using UserServiceApi.Models;

namespace UserServiceApi.Services
{
    public class UserService : IUserService
    {
        private readonly UserDbContext _dbContext;
        private readonly IConfiguration _configuration;


        public UserService(UserDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<AuthResultDto> RegisterAsync(RegisterDto registerDto)
        {
            if (await _dbContext.Users.AnyAsync(u => u.Email == registerDto.Email))
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "User already exists." };

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var user = new User
            {
                Email = registerDto.Email,
                PasswordHash = hashedPassword
            };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            var token = GenerateJwtToken(user);
            return new AuthResultDto { IsSuccess = true, Token = token };
        }

        public async Task<AuthResultDto> UpdateProfileAsync(int userId, UpdateProfileDto updateDto)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "User not found." };

            user.FirstName = updateDto.FirstName;
            user.LastName = updateDto.LastName;
            user.Email = updateDto.Email;

            await _dbContext.SaveChangesAsync();
            return new AuthResultDto { IsSuccess = true };
        }


        public async Task<AuthResultDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "Invalid credentials." };

            var token = GenerateJwtToken(user);
            return new AuthResultDto { IsSuccess = true, Token = token };
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(5000),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }

}
